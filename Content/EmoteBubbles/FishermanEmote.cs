﻿using FishermanNPC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace FishermanNPC.Content.EmoteBubbles
{
    public class FishermanEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.Town);
        }

        public override bool IsUnlocked() => FishermanSystem.fishermanUnlocked;
    }
}
