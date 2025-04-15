using FishermanNPC.Content.NPCs.Town;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace FishermanNPC.Common
{
    public class FishermanSystem : ModSystem
    {
        public static bool fishermanUnlocked = false;

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(fishermanUnlocked)] = fishermanUnlocked;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            fishermanUnlocked = tag.GetBool(nameof(fishermanUnlocked));
        }
    }
}
