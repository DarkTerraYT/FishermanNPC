using FishermanNPC.Content.Projectiles.Friendly;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishermanNPC.Content.Items
{
    public class FishGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            /*Main.RegisterItemAnimation(Type, new() { TicksPerFrame = 99999999, FrameCount = 2 });
            ItemID.Sets.AnimatesAsSoul[Type] = true;*/
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return true;
        }

        public override void SetDefaults()
        {
            Item.width = 54; 
            Item.height = 30; 
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.Green; 

            Item.useTime = 90; 
            Item.useAnimation = 10; 
            Item.useStyle = ItemUseStyleID.Shoot; 
            Item.autoReuse = true; 

            Item.DamageType = DamageClass.Ranged; 
            Item.damage = 22; 
            Item.knockBack = 5f;
            Item.noMelee = true; 

            Item.shoot = ModContent.ProjectileType<Fish>(); 
            Item.shootSpeed = 10f; 
        }
    }
}
