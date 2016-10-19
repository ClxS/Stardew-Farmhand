using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using xTile;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;

namespace Farmhand.API.Locations
{
    public class LocationUtilities
    {
        // Maps registered to be merged with a default map
        public static Dictionary<string, List<MapInformation>> RegisteredMaps { get; } = new Dictionary<string, List<MapInformation>>();

        // A stored list of merged maps, so the merging doesn't need to be done again
        public static Dictionary<string, Map> MergedMaps { get; } = new Dictionary<string, Map>();

        public static void RegisterMap(Mod owner, string assetName, MapInformation mapInformation)
        {
            if (!RegisteredMaps.ContainsKey(assetName))
            {
                RegisteredMaps[assetName] = new List<MapInformation>();
            }

            RegisteredMaps[assetName].Add(mapInformation);
        }

        public static Map MergeMaps(Map baseMap, string assetName)
        {
            // If there's no maps to inject, we can just skip all this
            if (!RegisteredMaps.ContainsKey(assetName) || RegisteredMaps[assetName].Count == 0) { return baseMap; }

            // If we've already merged this map, just return that
            if(MergedMaps.ContainsKey(assetName)) { return MergedMaps[assetName]; }

            // An array of the maps that need to be injected into the base map
            MapInformation[] injectedMaps = RegisteredMaps[assetName].ToArray();

            // Merge the maps
            Map mergedMap = MergeMaps(baseMap, injectedMaps);

            // Store the merged map
            if (!MergedMaps.ContainsKey(assetName))
            {
                MergedMaps.Add(assetName, mergedMap);
            }
            else
            {
                MergedMaps[assetName] = mergedMap;
            }

            return mergedMap;
        }

        public static Map MergeMaps(Map baseMap, MapInformation[] injectedMaps)
        {
            // Check if any maps have the FullOverride tag
            foreach(var map in injectedMaps)
            {
                if(map.Override == MapOverride.FullOverride)
                {
                    if(injectedMaps.Length > 1)
                    {
                        Farmhand.Logging.Log.Warning($"Map merging conflict on map {map.Map.Id} in mod {map.Owner.ModSettings.Name} requested full override, but {injectedMaps.Length-1} other map(s) had changes! Using full override.");
                    }

                    return map.Map;
                }
            }

            // Since there are maps with changes to be made, and no maps requested full override, we merge!
            try
            {
                // Create the new mergedMap object that we'll be injecting everything into, use the base Id
                Map mergedMap = new Map(baseMap.Id);

                // Merge the tilsheets
                MergeTileSheets(ref mergedMap, baseMap, injectedMaps);

                // Merge the layers
                MergeLayers(ref mergedMap, baseMap, injectedMaps);

                // Merge map properties from the base
                MergeMapProperties(ref mergedMap, baseMap, injectedMaps);
                
                // Return the finished, merged map
                return mergedMap;
            }
            catch(Exception e)
            {
                Farmhand.Logging.Log.Exception($"Could not merge maps for {baseMap.Id}: ", e);
                return baseMap;
            }
        }

        // Merge the tilesheets from a baseMap and an array of injectedMaps into a mergedMap
        private static void MergeTileSheets(ref Map mergedMap, Map baseMap, MapInformation[] injectedMaps)
        {
            foreach (TileSheet baseSheet in baseMap.TileSheets)
            {
                // Create a new tilesheet from the base information
                TileSheet mergedSheet = new TileSheet(baseSheet.Id, mergedMap, baseSheet.ImageSource, baseSheet.SheetSize, baseSheet.TileSize);

                // Merge properties into the tilesheet
                List<IPropertyCollection> propertyInjectingTileSheets = new List<IPropertyCollection>();
                foreach (var map in injectedMaps)
                {
                    // Identify the tilesheet in this injected map that matches the one currently being injected into the mergedMap
                    propertyInjectingTileSheets.Add(GetMatchingTileSheetInList(mergedSheet, map.Map.TileSheets, map).Properties);
                }
                MergeProperties(mergedSheet, propertyInjectingTileSheets, baseSheet);

                // Add the newly merged tilesheet into the merging map
                mergedMap.AddTileSheet(mergedSheet);
            }

            // Inject addtional tilesheets, currently if an injecting map adds a tilesheet, and another injecting map adds another sheet with the same texture(but perhaps different properties), it will throw a conflict.
            foreach (var map in injectedMaps)
            {
                // Check each tilesheet in this injecting map
                foreach (var injectingSheet in map.Map.TileSheets)
                {
                    // Check if this tilesheet exists already in the merged map, if it's already there we don't need to add it
                    bool alreadyExists = false;
                    foreach (TileSheet mergedSheet in mergedMap.TileSheets)
                    {
                        if (MatchTileSheetsByTexture(injectingSheet, mergedSheet))
                        {
                            alreadyExists = true;
                        }
                    }

                    // If it's not in, add it
                    if (!alreadyExists)
                    {
                        // Create a new tilesheet from the injecting information
                        TileSheet mergedSheet = new TileSheet(injectingSheet.Id, mergedMap, injectingSheet.ImageSource, injectingSheet.SheetSize, injectingSheet.TileSize);

                        // Copy all the properties from this tilesheet
                        foreach (var property in injectingSheet.Properties)
                        {
                            mergedSheet.Properties.Add(property);
                        }

                        // Add this newly merged tilesheet into the merged map
                        mergedMap.AddTileSheet(mergedSheet);
                    }
                }
            }
        }

        // Merge the layers from a baseMap and several injectedMaps into a mergedMap
        private static void MergeLayers(ref Map mergedMap, Map baseMap, MapInformation[] injectedMaps)
        {
            // Find the largest layer size among all injecting maps, and use that value
            xTile.Dimensions.Size largestLayerSize = GetLargestLayerSize(baseMap, injectedMaps);

            // Luckily, when an index beyond the size of an xTile map layer is checked, it just returns null... which is already the indication that there's no tile there. Convenient!

            for (int l = 0; l < baseMap.Layers.Count; l++)
            {
                Layer baseLayer = baseMap.Layers[l];
                Layer mergedLayer = new Layer(baseLayer.Id, mergedMap, largestLayerSize, baseLayer.TileSize);

                mergedLayer.Description = baseLayer.Description;
                mergedLayer.Visible = baseLayer.Visible;

                // Merge the tiles in this layer
                for (int i = 0; i < mergedLayer.LayerHeight; i++)
                {
                    for (int j = 0; j < mergedLayer.LayerWidth; j++)
                    {
                        // check to make sure the base tile isn't null
                        if (baseLayer.Tiles[j, i] != null)
                        {
                            // Identify the tilesheet for this tile
                            TileSheet mergedTileSheet = GetMatchingTileSheetInList(baseLayer.Tiles[j, i].TileSheet, mergedMap.TileSheets, null);

                            // Iterrate over all injected maps, if the injected tile is different than the base tile, inject it
                            // Keep track of whether anything has injected this tile yet, if it has, throw a warning about the conflict
                            Tile tileToUse = baseLayer.Tiles[j, i];
                            string modDidOverride = null;
                            foreach (MapInformation injectedMapInfo in injectedMaps)
                            {
                                // Find the matching layer in this injected map that matches the merged layer we're working on
                                Layer injectedLayer = GetMatchingLayerInList(mergedLayer, injectedMapInfo.Map.Layers, injectedMapInfo);

                                // Check the map offset to get our injected map, if our local position is negative, we haven't reached it yet
                                int injectedIndexX = j - injectedMapInfo.OffsetX;
                                int injectedIndexY = i - injectedMapInfo.OffsetY;
                                if(injectedIndexX < 0 || injectedIndexY < 0)
                                {
                                    // Skip this injecting map for this index
                                    continue;
                                }

                                if (!CompareTiles(injectedLayer.Tiles[injectedIndexX, injectedIndexY], baseLayer.Tiles[j, i]))
                                {
                                    if (modDidOverride == null)
                                    {
                                        // Only keep track of what map made the change if it wasn't a soft merging map
                                        if (injectedMapInfo.Override != MapOverride.SoftMerge)
                                        {
                                            modDidOverride = injectedMapInfo.Owner.ModSettings.Name;
                                        }
                                        tileToUse = injectedLayer.Tiles[injectedIndexX, injectedIndexY];
                                    }
                                    else
                                    {
                                        Farmhand.Logging.Log.Warning($"Map merging conflict on map {baseMap.Id}, {modDidOverride} and {injectedMapInfo.Owner.ModSettings.Name} both attempted to alter {j},{i} on layer {baseLayer.Id}, using first.");
                                    }
                                }

                                if (tileToUse != null)
                                {
                                    // Create either a new static or animated tile
                                    if (tileToUse is StaticTile)
                                    {
                                        StaticTile staticTileToUse = tileToUse as StaticTile;

                                        mergedLayer.Tiles[j, i] = new StaticTile(mergedLayer, mergedTileSheet, staticTileToUse.BlendMode, staticTileToUse.TileIndex);
                                    }
                                    else if (tileToUse is AnimatedTile)
                                    {
                                        AnimatedTile animatedTileToUse = tileToUse as AnimatedTile;

                                        // Get the frames as static tiles
                                        StaticTile[] mergedFrames = new StaticTile[animatedTileToUse.TileFrames.Length];
                                        for (int f = 0; f < mergedFrames.Length; f++)
                                        {
                                            mergedFrames[f] = new StaticTile(mergedLayer, mergedTileSheet, animatedTileToUse.TileFrames[f].BlendMode, animatedTileToUse.TileFrames[f].TileIndex);
                                        }

                                        mergedLayer.Tiles[j, i] = new AnimatedTile(mergedLayer, mergedFrames, animatedTileToUse.FrameInterval);
                                    }
                                    else
                                    {
                                        throw new Exception($"Base tile type in layer {baseLayer.Id} at {j},{i} unknown!");
                                    }
                                }
                                else
                                {
                                    mergedLayer.Tiles[j, i] = tileToUse;
                                }
                            }
                        }
                        else
                        {
                            mergedLayer.Tiles[j, i] = null;
                            TileSheet mergedTileSheet = null;

                            // Iterrate over all injected maps, if the injected tile is different than the base tile, inject it
                            // Keep track of whether anything has injected this tile yet, if it has, throw a warning about the conflict
                            Tile tileToUse = baseLayer.Tiles[j, i];
                            string modDidOverride = null;
                            foreach (MapInformation injectedMapInfo in injectedMaps)
                            {
                                // Find the matching layer in this injected map that matches the merged layer we're working on
                                Layer injectedLayer = GetMatchingLayerInList(mergedLayer, injectedMapInfo.Map.Layers, injectedMapInfo);

                                // If the base tile is null, then the only thing that makes them not equal is not being null
                                if (injectedLayer.Tiles[j, i] != null)
                                {
                                    if (modDidOverride == null)
                                    {
                                        // Only keep track of what map made the change if it wasn't a soft merging map
                                        if (injectedMapInfo.Override != MapOverride.SoftMerge)
                                        {
                                            modDidOverride = injectedMapInfo.Owner.ModSettings.Name;
                                        }
                                        tileToUse = injectedLayer.Tiles[j, i];

                                        // Get the tilesheet from the injected tile
                                        if (tileToUse.TileSheet != null)
                                        {
                                            mergedTileSheet = GetMatchingTileSheetInList(tileToUse.TileSheet, mergedMap.TileSheets, injectedMapInfo);
                                        }
                                    }
                                    else
                                    {
                                        Farmhand.Logging.Log.Warning($"Map merging conflict on map {baseMap.Id}, {modDidOverride} and {injectedMapInfo.Owner.ModSettings.Name} both attempted to alter {j},{i} on layer {baseLayer.Id}, using first.");
                                    }
                                }
                            }

                            if (tileToUse != null && mergedTileSheet != null)
                            {
                                // Create either a new static or animated tile
                                if (tileToUse is StaticTile)
                                {
                                    StaticTile staticTileToUse = tileToUse as StaticTile;

                                    mergedLayer.Tiles[j, i] = new StaticTile(mergedLayer, mergedTileSheet, staticTileToUse.BlendMode, staticTileToUse.TileIndex);
                                }
                                else if (tileToUse is AnimatedTile)
                                {
                                    AnimatedTile animatedTileToUse = tileToUse as AnimatedTile;

                                    // Get the frames as static tiles
                                    StaticTile[] mergedFrames = new StaticTile[animatedTileToUse.TileFrames.Length];
                                    for (int f = 0; f < mergedFrames.Length; f++)
                                    {
                                        mergedFrames[f] = new StaticTile(mergedLayer, mergedTileSheet, animatedTileToUse.TileFrames[f].BlendMode, animatedTileToUse.TileFrames[f].TileIndex);
                                    }

                                    mergedLayer.Tiles[j, i] = new AnimatedTile(mergedLayer, mergedFrames, animatedTileToUse.FrameInterval);
                                }
                                else
                                {
                                    throw new Exception($"Base tile type in layer {baseLayer.Id} at {j},{i} unknown!");
                                }
                            }
                            else
                            {
                                mergedLayer.Tiles[j, i] = null;
                            }
                        }
                    }
                }

                // Merge the layer properties into this layer
                List<IPropertyCollection> propertyInjectingLayers = new List<IPropertyCollection>();
                foreach (var map in injectedMaps)
                {
                    // Identify the layer in this injected map that matches the one currently being injected into the mergedMap
                    propertyInjectingLayers.Add(GetMatchingLayerInList(mergedLayer, map.Map.Layers, map).Properties);
                }
                MergeProperties(mergedLayer, propertyInjectingLayers, baseLayer);

                // Add the merged layer to the merged map
                mergedMap.AddLayer(mergedLayer);
            }
        }

        // Merge the map properties from a baseMap and an array of injectedMaps into a mergedMap
        private static void MergeMapProperties(ref Map mergedMap, Map baseMap, MapInformation[] injectedMaps)
        {
            List<IPropertyCollection> propertyInjectingMaps = new List<IPropertyCollection>();
            foreach (var map in injectedMaps)
            {
                propertyInjectingMaps.Add(map.Map.Properties);
            }
            MergeProperties(mergedMap, propertyInjectingMaps, baseMap);
        }

        // Merge properties of a baseComponent and several injecting components into a single component
        private static void MergeProperties(Component toInject, List<IPropertyCollection> injectingComponents, Component baseComponent)
        {
            // Warps need to be merged carefully if this component is a map, these dictionarys will help that happen
            // The key is the warp tile location, the value is the warp target
            Dictionary<string, string> baseWarps = new Dictionary<string, string>();
            Dictionary<string, string> injectedWarps = new Dictionary<string, string>();

            // Inject component properties from the base
            foreach (var property in baseComponent.Properties)
            {
                // If this is the map warp property, 
                if (toInject is Map && property.Key.Equals("Warp"))
                {
                    string warpString = property.Value;
                    string[] rawWarpSplit = warpString.Split(' ');

                    for(int i=0; i<rawWarpSplit.Length; i+=5)
                    {
                        baseWarps.Add($"{rawWarpSplit[i]} {rawWarpSplit[i + 1]}", $"{rawWarpSplit[i + 2]} {rawWarpSplit[i + 3]} {rawWarpSplit[i + 4]}");
                    }
                }
                else
                {
                    toInject.Properties[property.Key] = property.Value;
                }
            }

            // Inject component properties from all mods
            Dictionary<string, bool> changedProperties = new Dictionary<string, bool>();
            foreach (var injectingComponent in injectingComponents)
            {
                // Check for properties that the injecting component has, but the injected component doesn't(or has a different value of)
                foreach (var injectedProperty in injectingComponent)
                {
                    // This is a special merge case, the list of warps needs to be merged very careful
                    if(toInject is Map && injectedProperty.Key.Equals("Warp"))
                    {
                        string warpString = injectedProperty.Value;
                        string[] rawWarpSplit = warpString.Split(' ');

                        for (int i = 0; i < rawWarpSplit.Length; i += 5)
                        {
                            string newWarpKey = $"{rawWarpSplit[i]} {rawWarpSplit[i + 1]}";
                            string newWarpValue = $"{rawWarpSplit[i + 2]} {rawWarpSplit[i + 3]} {rawWarpSplit[i + 4]}";
                            
                            // Check the base list to see if a warp by this key is already registered
                            if(baseWarps.ContainsKey(newWarpKey))
                            {
                                // If the base warp value is not equal to the one we just found, inject it
                                if(!baseWarps[newWarpKey].Equals(newWarpValue))
                                {
                                    // Check if any other maps have already injected this warp tile
                                    if (!injectedWarps.ContainsKey(newWarpKey))
                                    {
                                        injectedWarps.Add(newWarpKey, newWarpValue);
                                    }
                                    else
                                    {
                                        Farmhand.Logging.Log.Warning($"Map merging conflict on warp {newWarpKey}, using {injectedWarps[newWarpKey]}");
                                    }
                                }
                            }
                            // If this warp tile isn't in the base list, inject it
                            else
                            {
                                // Check if any other maps have already injected this warp tile
                                if (!injectedWarps.ContainsKey(newWarpKey))
                                {
                                    injectedWarps.Add(newWarpKey, newWarpValue);
                                }
                                else
                                {
                                    Farmhand.Logging.Log.Warning($"Map merging conflict on warp {newWarpKey}, using {injectedWarps[newWarpKey]}");
                                }
                            }
                        }
                    }
                    // Properties the injected component already has, and the injecting component may want to edit
                    else if (toInject.Properties[injectedProperty.Key] != null)
                    {
                        if (!CompareProperties(toInject.Properties[injectedProperty.Key], injectingComponent[injectedProperty.Key]))
                        {
                            if (!changedProperties[injectedProperty.Key])
                            {
                                toInject.Properties[injectedProperty.Key] = injectingComponent[injectedProperty.Key];
                                changedProperties[injectedProperty.Key] = true;
                            }
                            else
                            {
                                Farmhand.Logging.Log.Warning($"Map merging conflict on property {injectedProperty.Key}, using {toInject.Properties[injectedProperty.Key].ToString()}");
                            }
                        }
                    }
                    // Properties the injected component does not have, and the injecting component may add
                    else
                    {
                        if (!changedProperties[injectedProperty.Key])
                        {
                            toInject.Properties.Add(injectedProperty);
                            changedProperties[injectedProperty.Key] = true;
                        }
                        else
                        {
                            Farmhand.Logging.Log.Warning($"Map merging conflict on property {injectedProperty.Key}, using {toInject.Properties[injectedProperty.Key].ToString()}");
                        }
                    }
                }

                // Check for properties that the injecting map does NOT have, but the merging map DOES, which means it was probably removed during editing
                foreach (var property in toInject.Properties)
                {
                    if (injectingComponent[property.Key] == null)
                    {
                        if (!changedProperties[property.Key])
                        {
                            toInject.Properties.Remove(property);
                            changedProperties[property.Key] = true;
                        }
                        else
                        {
                            Farmhand.Logging.Log.Warning($"Map merging conflict on property {property.Key}, using {toInject.Properties[property.Key].ToString()}");
                        }
                    }
                }
            }

            // Now that the injecting is done, if this is a map, we need to finalize those warp injections
            if(toInject is Map && (baseWarps.Count != 0 || injectedWarps.Count != 0))
            {
                // This dictionary will hold our final warps
                Dictionary<string, string> finalWarps = new Dictionary<string, string>();

                // First insert the injected warps
                foreach(KeyValuePair<string, string> injectedWarp in injectedWarps)
                {
                    finalWarps.Add(injectedWarp.Key, injectedWarp.Value);
                }

                // Now insert the base warps
                foreach(KeyValuePair<string, string> baseWarp in baseWarps)
                {
                    if(!finalWarps.ContainsKey(baseWarp.Key))
                    {
                        finalWarps.Add(baseWarp.Key, baseWarp.Value);
                    }
                }

                // Create one final string from this
                string finalWarpString = "";
                foreach(KeyValuePair<string, string> finalWarp in finalWarps)
                {
                    finalWarpString += $"{finalWarp.Key} {finalWarp.Value} ";
                }
                finalWarpString = finalWarpString.Trim();

                // Inject the warp property
                toInject.Properties.Add("Warp", new PropertyValue(finalWarpString));
            }
        }

        public static xTile.Dimensions.Size GetLargestLayerSize(Map baseMap, MapInformation[] injectedMaps)
        {
            xTile.Dimensions.Size largestLayerSize = new xTile.Dimensions.Size();
            
            // First, check all base layers, for a base size value
            foreach (Layer baseLayer in baseMap.Layers)
            {
                if (baseLayer.LayerWidth > largestLayerSize.Width)
                {
                    largestLayerSize.Width = baseLayer.LayerWidth;
                }

                if (baseLayer.LayerHeight > largestLayerSize.Height)
                {
                    largestLayerSize.Height = baseLayer.LayerHeight;
                }
            }

            // Next, check all injecting layers for larger values
            foreach (MapInformation map in injectedMaps)
            {
                foreach (Layer injectingLayer in map.Map.Layers)
                {
                    if (injectingLayer.LayerWidth + map.OffsetX > largestLayerSize.Width)
                    {
                        largestLayerSize.Width = injectingLayer.LayerWidth + map.OffsetX;
                    }

                    if (injectingLayer.LayerHeight + map.OffsetY > largestLayerSize.Height)
                    {
                        largestLayerSize.Height = injectingLayer.LayerHeight + map.OffsetY;
                    }
                }
            }

            return largestLayerSize;
        }

        public static Layer GetMatchingLayerInList(Layer toMatch, ReadOnlyCollection<Layer> list, MapInformation map)
        {
            Layer matchingLayer = null;
            foreach (Layer injectingLayer in list)
            {
                if (MatchLayersById(toMatch, injectingLayer))
                {
                    matchingLayer = injectingLayer;
                }
            }
            if (matchingLayer == null)
            {
                throw new Exception($"Could not find matching layer {toMatch.Id} in mod {map.Owner.ModSettings.Name} in map {map.Map.Id}!");
            }

            return matchingLayer;
        }

        public static TileSheet GetMatchingTileSheetInList(TileSheet toMatch, ReadOnlyCollection<TileSheet> list, MapInformation map)
        {
            TileSheet matchingTileSheet = null;
            foreach (TileSheet injectingTileSheet in list)
            {
                if (MatchTileSheetsByTexture(toMatch, injectingTileSheet))
                {
                    matchingTileSheet = injectingTileSheet;
                }
            }
            if (matchingTileSheet == null)
            {
                throw new Exception($"Could not find matching tilesheet {toMatch.ImageSource} in mod {map.Owner.ModSettings.Name} in map {map.Map.Id}!");
            }

            return matchingTileSheet;
        }


        public static bool CompareTiles(Tile a, Tile b)
        {
            // If either is null, check if it's just one or the other, if they're not both null, they're not equal
            if((a==null || b==null) && ((a!=null && b==null) || (b!=null && a==null))) { return false; }

            // If they're both null, they're equal
            if (a == null && b == null) { return true; }

            if (!CompareLayers(a.Layer, b.Layer)) { return false; }

            if (a.BlendMode != b.BlendMode) { return false; }

            if (!CompareTileSheets(a.TileSheet, b.TileSheet)) { return false; }

            if(!ComparePropertySet(a.Properties, b.Properties)) { return false; }

            if(a is AnimatedTile || b is AnimatedTile)
            {
                if (a is AnimatedTile && !(b is AnimatedTile)) { return false; }
                if (b is AnimatedTile && !(a is AnimatedTile)) { return false; }

                AnimatedTile aAnim = (AnimatedTile) a;
                AnimatedTile bAnim = (AnimatedTile) b;

                if (aAnim.TileFrames.Length != bAnim.TileFrames.Length) { return false; }
                for(int i=0; i<aAnim.TileFrames.Length; i++)
                {
                    if (!CompareTiles(aAnim.TileFrames[i], aAnim.TileFrames[i])) { return false; }
                }
            }
            else if(a is StaticTile || b is StaticTile)
            {
                if (a is StaticTile && !(b is StaticTile)) { return false; }
                if (b is StaticTile && !(a is StaticTile)) { return false; }
            }

            return true;
        }

        public static bool CompareTileSheets(TileSheet a, TileSheet b)
        {
            if (!a.ImageSource.Equals(b.ImageSource)) { return false; }

            if (!ComparePropertySet(a.Properties, b.Properties)) { return false; }

            if (a.SheetHeight != b.SheetHeight) { return false; }

            if (a.SheetWidth != b.SheetWidth) { return false; }

            if (a.TileHeight != b.TileHeight) { return false; }

            if (a.TileWidth != b.TileWidth) { return false; }

            if (a.MarginHeight != b.MarginHeight) { return false; }

            if (a.MarginWidth != b.MarginWidth) { return false; }

            if (a.Spacing != b.Spacing) { return false; }

            if (a.TileCount != b.TileCount) { return false; }

            return true;
        }

        public static bool MatchTileSheetsByTexture(TileSheet a, TileSheet b)
        {
            if (!a.ImageSource.Equals(b.ImageSource)) { return false; }

            return true;
        }

        public static bool CompareLayers(Layer a, Layer b)
        {
            if (!a.Id.Equals(b.Id)) { return false; }

            if (a.LayerWidth != b.LayerWidth) { return false; }

            if (a.LayerHeight != b.LayerHeight) { return false; }

            if (a.TileWidth != b.TileWidth) { return false; }

            if (a.TileHeight != b.TileHeight) { return false; }

            if (a.Visible != b.Visible) { return false; }

            return true;
        }

        public static bool MatchLayersById(Layer a, Layer b)
        {
            if (!a.Id.Equals(b.Id)) { return false; }

            return true;
        }

        public static bool ComparePropertySet(IPropertyCollection a, IPropertyCollection b)
        {
            if(a.Count != b.Count) { return false; }

            foreach(var property in a)
            {
                if(b[property.Key] != null)
                {
                    CompareProperties(a[property.Key], b[property.Key]);
                }
            }

            return true;
        }

        public static bool CompareProperties(PropertyValue aValue, PropertyValue bValue)
        {
            if(aValue.Type != bValue.Type) { return false; }

            // Compare the returned ToString values, since we know they're already the same type and don't have to worry about that
            if(!aValue.ToString().Equals(bValue.ToString())) { return false; }

            return true;
        }
    }
}
