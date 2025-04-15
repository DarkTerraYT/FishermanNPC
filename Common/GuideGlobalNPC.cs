using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishermanNPC.Common
{
    public class GuideGlobalNPC : GlobalNPC
    {
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.type == NPCID.Guide) 
            {
                Mod.Logger.Info($"x:{npc.width}, y:{npc.height}");
            }
        }
    }
}
