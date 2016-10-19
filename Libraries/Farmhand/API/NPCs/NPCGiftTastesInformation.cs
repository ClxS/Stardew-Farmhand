using System;
using System.Collections.Generic;
using Farmhand.Helpers;

namespace Farmhand.API.NPCs
{
    public class NPCGiftTastesInformation
    {
        public string Name { get; set; }

        public string LovesResponse { get; set; } = "";
        public List<int> LovesIds { get; set; } = new List<int>();

        public string HatesResponse { get; set; } = "";
        public List<int> HatesIds { get; set; } = new List<int>();

        public string LikesResponse { get; set; } = "";
        public List<int> LikesIds { get; set; } = new List<int>();

        public string DislikesResponse { get; set; } = "";
        public List<int> DislikesIds { get; set; } = new List<int>();

        public string NeutralResponse { get; set; } = "";
        public List<int> NeutralIds { get; set; } = new List<int>();

        public override string ToString()
        {
            var lovesIds = LovesIds.ToSpaceSeparatedString();
            var hatesIds = HatesIds.ToSpaceSeparatedString();
            var likesIds = LikesIds.ToSpaceSeparatedString();
            var dislikesIds = DislikesIds.ToSpaceSeparatedString();
            var neutralIds = NeutralIds.ToSpaceSeparatedString();

            return $"{LovesResponse}/{lovesIds}/{LikesResponse}/{likesIds}/{DislikesResponse}/{dislikesIds}/{HatesResponse}/{hatesIds}/{NeutralResponse}/{neutralIds}/";
        }
    }
}