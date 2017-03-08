namespace Farmhand.API.NPCs.GiftTastes
{
    using System.Collections.Generic;

    using Farmhand.Helpers;

    /// <summary>
    ///     NPC gift tastes information.
    /// </summary>
    public class GiftTastesInformation
    {
        /// <summary>
        ///     Gets or sets the response when receiving a loved item.
        /// </summary>
        public string LovesResponse { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the IDs of loved items.
        /// </summary>
        public List<int> LovesIds { get; set; } = new List<int>();

        /// <summary>
        ///     Gets or sets the response when receiving a hated item.
        /// </summary>
        public string HatesResponse { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the IDs of hated items.
        /// </summary>
        public List<int> HatesIds { get; set; } = new List<int>();

        /// <summary>
        ///     Gets or sets the response when receiving a liked item.
        /// </summary>
        public string LikesResponse { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the IDs of liked items.
        /// </summary>
        public List<int> LikesIds { get; set; } = new List<int>();

        /// <summary>
        ///     Gets or sets the response when receiving a disliked item.
        /// </summary>
        public string DislikesResponse { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the IDs of disliked items.
        /// </summary>
        public List<int> DislikesIds { get; set; } = new List<int>();

        /// <summary>
        ///     Gets or sets the response when receiving a neutral item.
        /// </summary>
        public string NeutralResponse { get; set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the IDs of neutral items.
        /// </summary>
        public List<int> NeutralIds { get; set; } = new List<int>();

        /// <summary>
        ///     Constructs the taste information as a game compatible string.
        /// </summary>
        /// <returns>
        ///     The information as a string.
        /// </returns>
        public string BuildTasteInformation()
        {
            var lovesIds = this.LovesIds.ToSpaceSeparatedString();
            var hatesIds = this.HatesIds.ToSpaceSeparatedString();
            var likesIds = this.LikesIds.ToSpaceSeparatedString();
            var dislikesIds = this.DislikesIds.ToSpaceSeparatedString();
            var neutralIds = this.NeutralIds.ToSpaceSeparatedString();

            return
                $"{this.LovesResponse}/{lovesIds}/{this.LikesResponse}/{likesIds}/{this.DislikesResponse}/{dislikesIds}/{this.HatesResponse}/{hatesIds}/{this.NeutralResponse}/{neutralIds}/";
        }
    }
}