// Author:
//   Jb Evain (jbevain@gmail.com)
// Copyright (c) 2008 - 2015 Jb Evain
// Copyright (c) 2008 - 2011 Novell, Inc.
// Licensed under the MIT/X11 license.
using SR = System.Reflection;

namespace Mono.Cecil
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    using Mono.Cecil.Cil;
    using Mono.Cecil.Metadata;
    using Mono.Cecil.PE;
    using Mono.Collections.Generic;

    public enum ReadingMode
    {
        Immediate = 1,

        Deferred = 2
    }

    public sealed class ReaderParameters
    {
        internal IAssemblyResolver assembly_resolver;

        internal IMetadataResolver metadata_resolver;

        public ReaderParameters()
            : this(ReadingMode.Deferred)
        {
        }

        public ReaderParameters(ReadingMode readingMode)
        {
            this.ReadingMode = readingMode;
        }

        public ReadingMode ReadingMode { get; set; }

        public IAssemblyResolver AssemblyResolver
        {
            get
            {
                return this.assembly_resolver;
            }

            set
            {
                this.assembly_resolver = value;
            }
        }

        public IMetadataResolver MetadataResolver
        {
            get
            {
                return this.metadata_resolver;
            }

            set
            {
                this.metadata_resolver = value;
            }
        }

        public Stream SymbolStream { get; set; }

        public ISymbolReaderProvider SymbolReaderProvider { get; set; }

        public bool ReadSymbols { get; set; }

#if !READ_ONLY
        internal IMetadataImporterProvider metadata_importer_provider;

#if !CF
        internal IReflectionImporterProvider reflection_importer_provider;
#endif
#endif

#if !READ_ONLY
        public IMetadataImporterProvider MetadataImporterProvider
        {
            get
            {
                return this.metadata_importer_provider;
            }

            set
            {
                this.metadata_importer_provider = value;
            }
        }

#if !CF
        public IReflectionImporterProvider ReflectionImporterProvider
        {
            get
            {
                return this.reflection_importer_provider;
            }

            set
            {
                this.reflection_importer_provider = value;
            }
        }
#endif
#endif
    }

#if !READ_ONLY

    public sealed class ModuleParameters
    {
        public ModuleParameters()
        {
            this.Kind = ModuleKind.Dll;
            this.Runtime = GetCurrentRuntime();
            this.Architecture = TargetArchitecture.I386;
        }

        public ModuleKind Kind { get; set; }

        public TargetRuntime Runtime { get; set; }

        public TargetArchitecture Architecture { get; set; }

        public IAssemblyResolver AssemblyResolver { get; set; }

        public IMetadataResolver MetadataResolver { get; set; }

        private static TargetRuntime GetCurrentRuntime()
        {
#if !CF
            return typeof(object).Assembly.ImageRuntimeVersion.ParseRuntime();
#else
			var corlib_version = typeof (object).Assembly.GetName ().Version;
			switch (corlib_version.Major) {
			case 1:
				return corlib_version.Minor == 0
					? TargetRuntime.Net_1_0
					: TargetRuntime.Net_1_1;
			case 2:
				return TargetRuntime.Net_2_0;
			case 4:
				return TargetRuntime.Net_4_0;
			default:
				throw new NotSupportedException ();
			}
#endif
        }

#if !READ_ONLY

#if !CF
#endif
#endif

#if !READ_ONLY
        public IMetadataImporterProvider MetadataImporterProvider { get; set; }

#if !CF
        public IReflectionImporterProvider ReflectionImporterProvider { get; set; }
#endif
#endif
    }

    public sealed class WriterParameters
    {
#if !SILVERLIGHT && !CF
#endif

        public Stream SymbolStream { get; set; }

        public ISymbolWriterProvider SymbolWriterProvider { get; set; }

        public bool WriteSymbols { get; set; }

#if !SILVERLIGHT && !CF
        public SR.StrongNameKeyPair StrongNameKeyPair { get; set; }
#endif
    }

#endif

    public sealed class ModuleDefinition : ModuleReference, ICustomAttributeProvider
    {
        public ResourceDirectory Win32ResourceDirectory
        {
            get
            {
                if (this.win32ResourceDirectory == null && this.Image != null)
                {
                    var rsrc = this.Image.GetSection(".rsrc");
                    if (rsrc != null && rsrc.Data.Length > 0)
                    {
                        this.win32ResourceDirectory = RsrcReader.ReadResourceDirectory(rsrc.Data, rsrc.VirtualAddress);
                    }
                }

                return this.win32ResourceDirectory ?? (this.win32ResourceDirectory = new ResourceDirectory());
            }

            set
            {
                this.win32ResourceDirectory = value;
            }
        }

        private readonly MetadataReader reader;

        internal AssemblyDefinition assembly;

        internal IAssemblyResolver assembly_resolver;

        private Collection<CustomAttribute> custom_attributes;

        private MethodDefinition entry_point;

        private Collection<ExportedType> exported_types;

        internal Image Image;

        internal ModuleKind kind;

        internal IMetadataResolver metadata_resolver;

        internal MetadataSystem MetadataSystem;

        private Collection<ModuleReference> modules;

        internal ReadingMode ReadingMode;

        private Collection<AssemblyNameReference> references;

        private Collection<Resource> resources;

        private TargetRuntime runtime;

        internal string runtime_version;

        internal ISymbolReader symbol_reader;

        internal ISymbolReaderProvider SymbolReaderProvider;

        internal TypeSystem type_system;

        private TypeDefinitionCollection types;

        private ResourceDirectory win32ResourceDirectory;

        internal byte[] Win32Resources;

        internal uint Win32RVA;

        internal ModuleDefinition()
        {
            this.MetadataSystem = new MetadataSystem();
            this.token = new MetadataToken(TokenType.Module, 1);
        }

        internal ModuleDefinition(Image image)
            : this()
        {
            this.Image = image;
            this.kind = image.Kind;
            this.RuntimeVersion = image.RuntimeVersion;
            this.Architecture = image.Architecture;
            this.Attributes = image.Attributes;
            this.Characteristics = image.Characteristics;
            this.FullyQualifiedName = image.FileName;

            this.reader = new MetadataReader(this);
        }

        public bool IsMain
        {
            get
            {
                return this.kind != ModuleKind.NetModule;
            }
        }

        public ModuleKind Kind
        {
            get
            {
                return this.kind;
            }

            set
            {
                this.kind = value;
            }
        }

        public TargetRuntime Runtime
        {
            get
            {
                return this.runtime;
            }

            set
            {
                this.runtime = value;
                this.runtime_version = this.runtime.RuntimeVersionString();
            }
        }

        public string RuntimeVersion
        {
            get
            {
                return this.runtime_version;
            }

            set
            {
                this.runtime_version = value;
                this.runtime = this.runtime_version.ParseRuntime();
            }
        }

        public TargetArchitecture Architecture { get; set; }

        public ModuleAttributes Attributes { get; set; }

        public ModuleCharacteristics Characteristics { get; set; }

        public string FullyQualifiedName { get; }

        public Guid Mvid { get; set; }

        internal bool HasImage
        {
            get
            {
                return this.Image != null;
            }
        }

        public bool HasSymbols
        {
            get
            {
                return this.symbol_reader != null;
            }
        }

        public ISymbolReader SymbolReader
        {
            get
            {
                return this.symbol_reader;
            }
        }

        public override MetadataScopeType MetadataScopeType
        {
            get
            {
                return MetadataScopeType.ModuleDefinition;
            }
        }

        public AssemblyDefinition Assembly
        {
            get
            {
                return this.assembly;
            }
        }

        public IAssemblyResolver AssemblyResolver
        {
            get
            {
                if (this.assembly_resolver == null)
                {
                    Interlocked.CompareExchange(ref this.assembly_resolver, new DefaultAssemblyResolver(), null);
                }

                return this.assembly_resolver;
            }
        }

        public IMetadataResolver MetadataResolver
        {
            get
            {
                if (this.metadata_resolver == null)
                {
                    Interlocked.CompareExchange(
                        ref this.metadata_resolver,
                        new MetadataResolver(this.AssemblyResolver),
                        null);
                }

                return this.metadata_resolver;
            }
        }

        public TypeSystem TypeSystem
        {
            get
            {
                if (this.type_system == null)
                {
                    Interlocked.CompareExchange(ref this.type_system, TypeSystem.CreateTypeSystem(this), null);
                }

                return this.type_system;
            }
        }

        public bool HasAssemblyReferences
        {
            get
            {
                if (this.references != null)
                {
                    return this.references.Count > 0;
                }

                return this.HasImage && this.Image.HasTable(Table.AssemblyRef);
            }
        }

        public Collection<AssemblyNameReference> AssemblyReferences
        {
            get
            {
                if (this.references != null)
                {
                    return this.references;
                }

                if (this.HasImage)
                {
                    return this.Read(ref this.references, this, (_, reader) => reader.ReadAssemblyReferences());
                }

                return this.references = new Collection<AssemblyNameReference>();
            }
        }

        public bool HasModuleReferences
        {
            get
            {
                if (this.modules != null)
                {
                    return this.modules.Count > 0;
                }

                return this.HasImage && this.Image.HasTable(Table.ModuleRef);
            }
        }

        public Collection<ModuleReference> ModuleReferences
        {
            get
            {
                if (this.modules != null)
                {
                    return this.modules;
                }

                if (this.HasImage)
                {
                    return this.Read(ref this.modules, this, (_, reader) => reader.ReadModuleReferences());
                }

                return this.modules = new Collection<ModuleReference>();
            }
        }

        public bool HasResources
        {
            get
            {
                if (this.resources != null)
                {
                    return this.resources.Count > 0;
                }

                if (this.HasImage)
                {
                    return this.Image.HasTable(Table.ManifestResource)
                           || this.Read(this, (_, reader) => reader.HasFileResource());
                }

                return false;
            }
        }

        public Collection<Resource> Resources
        {
            get
            {
                if (this.resources != null)
                {
                    return this.resources;
                }

                if (this.HasImage)
                {
                    return this.Read(ref this.resources, this, (_, reader) => reader.ReadResources());
                }

                return this.resources = new Collection<Resource>();
            }
        }

        public bool HasTypes
        {
            get
            {
                if (this.types != null)
                {
                    return this.types.Count > 0;
                }

                return this.HasImage && this.Image.HasTable(Table.TypeDef);
            }
        }

        public Collection<TypeDefinition> Types
        {
            get
            {
                if (this.types != null)
                {
                    return this.types;
                }

                if (this.HasImage)
                {
                    return this.Read(ref this.types, this, (_, reader) => reader.ReadTypes());
                }

                return this.types = new TypeDefinitionCollection(this);
            }
        }

        public bool HasExportedTypes
        {
            get
            {
                if (this.exported_types != null)
                {
                    return this.exported_types.Count > 0;
                }

                return this.HasImage && this.Image.HasTable(Table.ExportedType);
            }
        }

        public Collection<ExportedType> ExportedTypes
        {
            get
            {
                if (this.exported_types != null)
                {
                    return this.exported_types;
                }

                if (this.HasImage)
                {
                    return this.Read(ref this.exported_types, this, (_, reader) => reader.ReadExportedTypes());
                }

                return this.exported_types = new Collection<ExportedType>();
            }
        }

        public MethodDefinition EntryPoint
        {
            get
            {
                if (this.entry_point != null)
                {
                    return this.entry_point;
                }

                if (this.HasImage)
                {
                    return this.Read(ref this.entry_point, this, (_, reader) => reader.ReadEntryPoint());
                }

                return this.entry_point = null;
            }

            set
            {
                this.entry_point = value;
            }
        }

        internal object SyncRoot { get; } = new object();

        public bool HasDebugHeader
        {
            get
            {
                return this.Image != null && !this.Image.Debug.IsZero;
            }
        }
        
        #region ICustomAttributeProvider Members

        public bool HasCustomAttributes
        {
            get
            {
                if (this.custom_attributes != null)
                {
                    return this.custom_attributes.Count > 0;
                }

                return this.GetHasCustomAttributes(this);
            }
        }

        public Collection<CustomAttribute> CustomAttributes
        {
            get
            {
                return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this);
            }
        }

        #endregion

        public bool HasTypeReference(string fullName)
        {
            return this.HasTypeReference(string.Empty, fullName);
        }

        public bool HasTypeReference(string scope, string fullName)
        {
            CheckFullName(fullName);

            if (!this.HasImage)
            {
                return false;
            }

            return this.GetTypeReference(scope, fullName) != null;
        }

        public bool TryGetTypeReference(string fullName, out TypeReference type)
        {
            return this.TryGetTypeReference(string.Empty, fullName, out type);
        }

        public bool TryGetTypeReference(string scope, string fullName, out TypeReference type)
        {
            CheckFullName(fullName);

            if (!this.HasImage)
            {
                type = null;
                return false;
            }

            return (type = this.GetTypeReference(scope, fullName)) != null;
        }

        private TypeReference GetTypeReference(string scope, string fullname)
        {
            return this.Read(
                new Row<string, string>(scope, fullname),
                (row, reader) => reader.GetTypeReference(row.Col1, row.Col2));
        }

        public IEnumerable<TypeReference> GetTypeReferences()
        {
            if (!this.HasImage)
            {
                return Empty<TypeReference>.Array;
            }

            return this.Read(this, (_, reader) => reader.GetTypeReferences());
        }

        public IEnumerable<MemberReference> GetMemberReferences()
        {
            if (!this.HasImage)
            {
                return Empty<MemberReference>.Array;
            }

            return this.Read(this, (_, reader) => reader.GetMemberReferences());
        }

        public TypeReference GetType(string fullName, bool runtimeName)
        {
            return runtimeName ? TypeParser.ParseType(this, fullName) : this.GetType(fullName);
        }

        public TypeDefinition GetType(string fullName)
        {
            CheckFullName(fullName);

            var position = fullName.IndexOf('/');
            if (position > 0)
            {
                return this.GetNestedType(fullName);
            }

            return ((TypeDefinitionCollection)this.Types).GetType(fullName);
        }

        public TypeDefinition GetType(string @namespace, string name)
        {
            Mixin.CheckName(name);

            return ((TypeDefinitionCollection)this.Types).GetType(@namespace ?? string.Empty, name);
        }

        public IEnumerable<TypeDefinition> GetTypes()
        {
            return GetTypes(this.Types);
        }

        private static IEnumerable<TypeDefinition> GetTypes(Collection<TypeDefinition> types)
        {
            for (var i = 0; i < types.Count; i++)
            {
                var type = types[i];

                yield return type;

                if (!type.HasNestedTypes)
                {
                    continue;
                }

                foreach (var nested in GetTypes(type.NestedTypes))
                {
                    yield return nested;
                }
            }
        }

        private static void CheckFullName(string fullName)
        {
            if (fullName == null)
            {
                throw new ArgumentNullException("fullName");
            }

            if (fullName.Length == 0)
            {
                throw new ArgumentException();
            }
        }

        private TypeDefinition GetNestedType(string fullname)
        {
            var names = fullname.Split('/');
            var type = this.GetType(names[0]);

            if (type == null)
            {
                return null;
            }

            for (var i = 1; i < names.Length; i++)
            {
                var nested_type = type.GetNestedType(names[i]);
                if (nested_type == null)
                {
                    return null;
                }

                type = nested_type;
            }

            return type;
        }

        internal FieldDefinition Resolve(FieldReference field)
        {
            return this.MetadataResolver.Resolve(field);
        }

        internal MethodDefinition Resolve(MethodReference method)
        {
            return this.MetadataResolver.Resolve(method);
        }

        internal TypeDefinition Resolve(TypeReference type)
        {
            return this.MetadataResolver.Resolve(type);
        }

        public IMetadataTokenProvider LookupToken(int token)
        {
            return this.LookupToken(new MetadataToken((uint)token));
        }

        public IMetadataTokenProvider LookupToken(MetadataToken token)
        {
            return this.Read(token, (t, reader) => reader.LookupToken(t));
        }

        internal TRet Read<TItem, TRet>(TItem item, Func<TItem, MetadataReader, TRet> read)
        {
            lock (this.SyncRoot)
            {
                var position = this.reader.position;
                var context = this.reader.context;

                var ret = read(item, this.reader);

                this.reader.position = position;
                this.reader.context = context;

                return ret;
            }
        }

        internal TRet Read<TItem, TRet>(ref TRet variable, TItem item, Func<TItem, MetadataReader, TRet> read)
            where TRet : class
        {
            lock (this.SyncRoot)
            {
                if (variable != null)
                {
                    return variable;
                }

                var position = this.reader.position;
                var context = this.reader.context;

                var ret = read(item, this.reader);

                this.reader.position = position;
                this.reader.context = context;

                return variable = ret;
            }
        }

        public ImageDebugDirectory GetDebugHeader(out byte[] header)
        {
            if (!this.HasDebugHeader)
            {
                throw new InvalidOperationException();
            }

            return this.Image.GetDebugHeader(out header);
        }

        private void ProcessDebugHeader()
        {
            if (!this.HasDebugHeader)
            {
                return;
            }

            byte[] header;
            var directory = this.GetDebugHeader(out header);

            if (!this.symbol_reader.ProcessDebugHeader(directory, header))
            {
                throw new InvalidOperationException();
            }
        }

        public void ReadSymbols()
        {
            if (string.IsNullOrEmpty(this.FullyQualifiedName))
            {
                throw new InvalidOperationException();
            }

            var provider = SymbolProvider.GetPlatformReaderProvider();
            if (provider == null)
            {
                throw new InvalidOperationException();
            }

            this.ReadSymbols(provider.GetSymbolReader(this, this.FullyQualifiedName));
        }

        public void ReadSymbols(ISymbolReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this.symbol_reader = reader;

            this.ProcessDebugHeader();
        }

        public static ModuleDefinition ReadModule(string fileName)
        {
            return ReadModule(fileName, new ReaderParameters(ReadingMode.Deferred));
        }

        public static ModuleDefinition ReadModule(Stream stream)
        {
            return ReadModule(stream, new ReaderParameters(ReadingMode.Deferred));
        }

        public static ModuleDefinition ReadModule(string fileName, ReaderParameters parameters)
        {
            using (var stream = GetFileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ReadModule(stream, parameters);
            }
        }

        public static ModuleDefinition ReadModule(Stream stream, ReaderParameters parameters)
        {
            Mixin.CheckStream(stream);
            if (!stream.CanRead || !stream.CanSeek)
            {
                throw new ArgumentException();
            }

            Mixin.CheckParameters(parameters);

            return ModuleReader.CreateModuleFrom(ImageReader.ReadImageFrom(stream), parameters);
        }

        private static Stream GetFileStream(string fileName, FileMode mode, FileAccess access, FileShare share)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (fileName.Length == 0)
            {
                throw new ArgumentException();
            }

            return new FileStream(fileName, mode, access, share);
        }

#if !READ_ONLY
#if !CF
        internal IReflectionImporter reflection_importer;
#endif

        internal IMetadataImporter metadata_importer;
#endif

#if !READ_ONLY
#if !CF
        internal IReflectionImporter ReflectionImporter
        {
            get
            {
                if (this.reflection_importer == null)
                {
                    Interlocked.CompareExchange(ref this.reflection_importer, new ReflectionImporter(this), null);
                }

                return this.reflection_importer;
            }
        }
#endif

        internal IMetadataImporter MetadataImporter
        {
            get
            {
                if (this.metadata_importer == null)
                {
                    Interlocked.CompareExchange(ref this.metadata_importer, new MetadataImporter(this), null);
                }

                return this.metadata_importer;
            }
        }
#endif

#if !READ_ONLY

        private static void CheckContext(IGenericParameterProvider context, ModuleDefinition module)
        {
            if (context == null)
            {
                return;
            }

            if (context.Module != module)
            {
                throw new ArgumentException();
            }
        }

#if !CF
        
        public TypeReference Import(Type type)
        {
            return this.ImportReference(type, null);
        }

        public TypeReference ImportReference(Type type)
        {
            return this.ImportReference(type, null);
        }
        
        public TypeReference Import(Type type, IGenericParameterProvider context)
        {
            return this.ImportReference(type, context);
        }

        public TypeReference ImportReference(Type type, IGenericParameterProvider context)
        {
            Mixin.CheckType(type);
            CheckContext(context, this);

            return this.ReflectionImporter.ImportReference(type, context);
        }
        
        public FieldReference Import(SR.FieldInfo field)
        {
            return this.ImportReference(field, null);
        }
        
        public FieldReference Import(SR.FieldInfo field, IGenericParameterProvider context)
        {
            return this.ImportReference(field, context);
        }

        public FieldReference ImportReference(SR.FieldInfo field)
        {
            return this.ImportReference(field, null);
        }

        public FieldReference ImportReference(SR.FieldInfo field, IGenericParameterProvider context)
        {
            Mixin.CheckField(field);
            CheckContext(context, this);

            return this.ReflectionImporter.ImportReference(field, context);
        }
        
        public MethodReference Import(SR.MethodBase method)
        {
            return this.ImportReference(method, null);
        }
        
        public MethodReference Import(SR.MethodBase method, IGenericParameterProvider context)
        {
            return this.ImportReference(method, context);
        }

        public MethodReference ImportReference(SR.MethodBase method)
        {
            return this.ImportReference(method, null);
        }

        public MethodReference ImportReference(SR.MethodBase method, IGenericParameterProvider context)
        {
            Mixin.CheckMethod(method);
            CheckContext(context, this);

            return this.ReflectionImporter.ImportReference(method, context);
        }

#endif
        
        public TypeReference Import(TypeReference type)
        {
            return this.ImportReference(type, null);
        }
        
        public TypeReference Import(TypeReference type, IGenericParameterProvider context)
        {
            return this.ImportReference(type, context);
        }

        public TypeReference ImportReference(TypeReference type)
        {
            return this.ImportReference(type, null);
        }

        public TypeReference ImportReference(TypeReference type, IGenericParameterProvider context)
        {
            Mixin.CheckType(type);

            if (type.Module == this)
            {
                return type;
            }

            CheckContext(context, this);

            return this.MetadataImporter.ImportReference(type, context);
        }
        
        public FieldReference Import(FieldReference field)
        {
            return this.ImportReference(field, null);
        }
        
        public FieldReference Import(FieldReference field, IGenericParameterProvider context)
        {
            return this.ImportReference(field, context);
        }

        public FieldReference ImportReference(FieldReference field)
        {
            return this.ImportReference(field, null);
        }

        public FieldReference ImportReference(FieldReference field, IGenericParameterProvider context)
        {
            Mixin.CheckField(field);

            if (field.Module == this)
            {
                return field;
            }

            CheckContext(context, this);

            return this.MetadataImporter.ImportReference(field, context);
        }
        
        public MethodReference Import(MethodReference method)
        {
            return this.ImportReference(method, null);
        }
        
        public MethodReference Import(MethodReference method, IGenericParameterProvider context)
        {
            return this.ImportReference(method, context);
        }

        public MethodReference ImportReference(MethodReference method)
        {
            return this.ImportReference(method, null);
        }

        public MethodReference ImportReference(MethodReference method, IGenericParameterProvider context)
        {
            Mixin.CheckMethod(method);

            if (method.Module == this)
            {
                return method;
            }

            CheckContext(context, this);

            return this.MetadataImporter.ImportReference(method, context);
        }

        public void ImportWin32Resources(ModuleDefinition source)
        {
            var rsrc = source.Image.GetSection(".rsrc");
            var raw_resources = new byte[rsrc.Data.Length];
            Buffer.BlockCopy(rsrc.Data, 0, raw_resources, 0, rsrc.Data.Length);
            this.Win32Resources = raw_resources;
            this.Win32RVA = rsrc.VirtualAddress;
        }

#endif

#if !READ_ONLY

        public static ModuleDefinition CreateModule(string name, ModuleKind kind)
        {
            return CreateModule(name, new ModuleParameters { Kind = kind });
        }

        public static ModuleDefinition CreateModule(string name, ModuleParameters parameters)
        {
            Mixin.CheckName(name);
            Mixin.CheckParameters(parameters);

            var module = new ModuleDefinition
                             {
                                 Name = name,
                                 kind = parameters.Kind,
                                 Runtime = parameters.Runtime,
                                 Architecture = parameters.Architecture,
                                 Mvid = Guid.NewGuid(),
                                 Attributes = ModuleAttributes.ILOnly,
                                 Characteristics = (ModuleCharacteristics)0x8540
                             };

            if (parameters.AssemblyResolver != null)
            {
                module.assembly_resolver = parameters.AssemblyResolver;
            }

            if (parameters.MetadataResolver != null)
            {
                module.metadata_resolver = parameters.MetadataResolver;
            }

#if !READ_ONLY
            if (parameters.MetadataImporterProvider != null)
            {
                module.metadata_importer = parameters.MetadataImporterProvider.GetMetadataImporter(module);
            }

#if !CF
            if (parameters.ReflectionImporterProvider != null)
            {
                module.reflection_importer = parameters.ReflectionImporterProvider.GetReflectionImporter(module);
            }

#endif
#endif

            if (parameters.Kind != ModuleKind.NetModule)
            {
                var assembly = new AssemblyDefinition();
                module.assembly = assembly;
                module.assembly.Name = CreateAssemblyName(name);
                assembly.main_module = module;
            }

            module.Types.Add(new TypeDefinition(string.Empty, "<Module>", TypeAttributes.NotPublic));

            return module;
        }

        private static AssemblyNameDefinition CreateAssemblyName(string name)
        {
            if (name.EndsWith(".dll") || name.EndsWith(".exe"))
            {
                name = name.Substring(0, name.Length - 4);
            }

            return new AssemblyNameDefinition(name, Mixin.ZeroVersion);
        }

#endif

#if !READ_ONLY

        public void Write(string fileName)
        {
            this.Write(fileName, new WriterParameters());
        }

        public void Write(Stream stream)
        {
            this.Write(stream, new WriterParameters());
        }

        public void Write(string fileName, WriterParameters parameters)
        {
            using (var stream = GetFileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                this.Write(stream, parameters);
            }
        }

        public void Write(Stream stream, WriterParameters parameters)
        {
            Mixin.CheckStream(stream);
            if (!stream.CanWrite || !stream.CanSeek)
            {
                throw new ArgumentException();
            }

            Mixin.CheckParameters(parameters);

            ModuleWriter.WriteModuleTo(this, stream, parameters);
        }

#endif
    }

    internal static partial class Mixin
    {
        public static void CheckStream(object stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
        }

        public static void CheckParameters(object parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }
        }

        public static bool HasImage(this ModuleDefinition self)
        {
            return self != null && self.HasImage;
        }

        public static bool IsCoreLibrary(this ModuleDefinition module)
        {
            if (module.Assembly == null)
            {
                return false;
            }

            var assembly_name = module.Assembly.Name.Name;

            if (assembly_name != "mscorlib" && assembly_name != "System.Runtime")
            {
                return false;
            }

            if (module.HasImage && !module.MetadataSystem.HasSystemObject)
            {
                return false;
            }

            return true;
        }

        public static string GetFullyQualifiedName(this Stream self)
        {
#if !SILVERLIGHT
            var file_stream = self as FileStream;
            if (file_stream == null)
            {
                return string.Empty;
            }

            return Path.GetFullPath(file_stream.Name);
#else
			return string.Empty;
#endif
        }

        public static TargetRuntime ParseRuntime(this string self)
        {
            switch (self[1])
            {
                case '1':
                    return self[3] == '0' ? TargetRuntime.Net_1_0 : TargetRuntime.Net_1_1;
                case '2':
                    return TargetRuntime.Net_2_0;
                case '4':
                default:
                    return TargetRuntime.Net_4_0;
            }
        }

        public static string RuntimeVersionString(this TargetRuntime runtime)
        {
            switch (runtime)
            {
                case TargetRuntime.Net_1_0:
                    return "v1.0.3705";
                case TargetRuntime.Net_1_1:
                    return "v1.1.4322";
                case TargetRuntime.Net_2_0:
                    return "v2.0.50727";
                case TargetRuntime.Net_4_0:
                default:
                    return "v4.0.30319";
            }
        }

#if !READ_ONLY

        public static void CheckType(object type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
        }

        public static void CheckField(object field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
        }

        public static void CheckMethod(object method)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
        }

#endif
    }
}