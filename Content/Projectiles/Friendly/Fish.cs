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

namespace FishermanNPC.Content.Projectiles.Friendly
{
    public class Fish : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);

            Projectile.width = 18;
            Projectile.height = 38;

            Projectile.penetrate = 10;
        }

        float[] ai => Projectile.ai;

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.velocity = Projectile.oldVelocity * -0.8f;
            Projectile.direction *= -1;
            SoundEngine.PlaySound(SoundID.NPCDeath28, Projectile.position);
        }

        public override void OnHitPlayer(Player player, Player.HurtInfo hurtInfo)
        {
            Projectile.velocity = Projectile.oldVelocity * -0.8f;
            Projectile.direction *= -1;

            SoundEngine.PlaySound(SoundID.NPCDeath28, Projectile.position);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ai[2]++;
            Projectile.velocity = oldVelocity * -0.8f;
            Projectile.direction *= -1;
            SoundEngine.PlaySound(SoundID.NPCDeath28, Projectile.position);

            return ai[2] > 10;
        }

    }
}
