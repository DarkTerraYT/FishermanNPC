using FishermanNPC.Common;
using FishermanNPC.Content.EmoteBubbles;
using FishermanNPC.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace FishermanNPC.Content.NPCs.Town
{
    [AutoloadHead]
    public class Fisherman : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Guide];

            NPCID.Sets.ExtraFramesCount[Type] = NPCID.Sets.ExtraFramesCount[NPCID.Guide];
            NPCID.Sets.AttackFrameCount[Type] = NPCID.Sets.AttackFrameCount[NPCID.Guide];
            NPCID.Sets.DangerDetectRange[Type] = 500;
            NPCID.Sets.AttackType[Type] = 0;
            NPCID.Sets.AttackTime[Type] = 60;
            NPCID.Sets.AttackAverageChance[Type] = 50;
            NPCID.Sets.HatOffsetY[Type] = NPCID.Sets.HatOffsetY[NPCID.Guide];

            NPCID.Sets.FaceEmote[Type] = ModContent.EmoteBubbleType<FishermanEmote>();

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Direction = 1 
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Dislike) 
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Love) 
                .SetNPCAffection(NPCID.Angler, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Mechanic, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Steampunker, AffectionLevel.Dislike) 
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) 
            ;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 15;
            knockback = 1f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 20;
            randExtraCooldown = 10;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ModContent.ProjectileType<Fish>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 8f;
            randomOffset = 2f;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true; 
            NPC.friendly = true; 
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.Guide;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            if(FishermanSystem.fishermanUnlocked) return true;

            foreach(var plr in Main.ActivePlayers)
            {
                if(plr.inventory.Any(i => i.fishingPole > 0))
                {
                    return true;
                }
            }

            return false;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("Mods.FishermanNPC.Bestiary.Fisherman"),
            ]);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                string variant = "";
                if (NPC.IsShimmerVariant) variant += "_Shimmer";
                if (NPC.altTexture == 1) variant += "_Party";
                int hatGore = NPC.GetPartyHatGore();
                int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
                int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
                int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

                if (hatGore > 0)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
                }
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
            }
        }
        public override void OnSpawn(IEntitySource source)
        {
            if (source is EntitySource_SpawnNPC)
            {
                // A TownNPC is "unlocked" once it successfully spawns into the world.
                FishermanSystem.fishermanUnlocked = true;
            }
        }
        public override List<string> SetNPCNameList()
        {
            return new List<string>() {
                "Portagee Leadhead",
                "Skinny Fishmore",
                "Cap'n Sharkbait",
                "Lunky Chunkbait",
                "Stinky Wetline"
            };
        }

        int chatTimes = 0;

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
            if (partyGirl >= 0 && Main.rand.NextBool(4))
            {
                chat.Add(Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.PartyGirlDialogue", Main.npc[partyGirl].GivenName));
            }
            chat.Add(Language.GetTextValue("Mods.FishermanNPC.Dialogue1"));
            chat.Add(Language.GetTextValue("Mods.FishermanNPC.Dialogue2"));
            chat.Add(Language.GetTextValue("Mods.FishermanNPC.Dialogue3"));
            chat.Add(Language.GetTextValue("Mods.FishermanNPC.Dialogue4"));
            chat.Add(Language.GetTextValue("Mods.FishermanNPC.RareDialogue"), 0.1f);

            chatTimes++;
            if (chatTimes >= 10)
            {
                chat.Add(Language.GetTextValue("Mods.FishermanNPC.TalkALot"));
            }
            if(Main.raining)
            {
                chat.Add(Language.GetTextValue("Mods.FishermanNPC.Rainy"));
            }

            string chosenChat = chat;

            return chosenChat;
        }
        public override void SetChatButtons(ref string button, ref string button2)
        { 
            button = Language.GetTextValue("LegacyInterface.28");
        }
        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            if(firstButton)
            {
                shopName = "Shop";
            }
        }

        public override void AddShops()
        {
            var shop = new NPCShop(Type);
            shop.Add(ItemID.Fish);
            shop.Add(new Item(ItemID.JojaCola) { shopCustomPrice = Item.buyPrice(silver: 20) });
            shop.Add(ItemID.Worm);
            shop.Add(ItemID.ReaverShark, Condition.DownedEowOrBoc);
            shop.Add(ItemID.SawtoothShark, Condition.DownedEowOrBoc);
            shop.Add(ItemID.FrogLeg, Condition.DownedEowOrBoc);
            shop.Add(ItemID.BalloonPufferfish, Condition.DownedEowOrBoc);
            shop.Add(ItemID.Toxikarp, Condition.CorruptWorld, Condition.DownedMechBossAny);
            shop.Add(ItemID.Bladetongue, Condition.CrimsonWorld, Condition.DownedMechBossAny);
            shop.Add(ItemID.CrystalSerpent, Condition.DownedMechBossAny);
            shop.Add(ItemID.CombatBook, Condition.BloodMoon, Condition.Hardmode);
            shop.Add(ItemID.WoodFishingPole);
            shop.Add(ItemID.ReinforcedFishingPole);
            shop.Add(ItemID.FisherofSouls, Condition.CorruptWorld);
            shop.Add(ItemID.Fleshcatcher, Condition.CrimsonWorld);
            shop.Add(4325, Condition.BloodMoonOrHardmode);
            shop.Add(ItemID.FiberglassFishingPole, Condition.DownedQueenBee);
            shop.Add(ItemID.HotlineFishingHook, Condition.DownedMechBossAny);
            shop.Add(ItemID.GoldenFishingRod, Condition.AnglerQuestsFinishedOver(30));
            shop.Register();
        }
    }
}
