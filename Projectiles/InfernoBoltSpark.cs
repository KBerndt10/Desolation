using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Projectiles
{
    public class InfernoBoltSpark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Inferno Bolt");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.light = 1.4f;
        }

        private float AI0
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        private float vX
        {
            get => projectile.velocity.X;
            set => projectile.velocity.X = value;
        }

        private float vY
        {
            get => projectile.velocity.Y;
            set => projectile.velocity.Y = value;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.myPlayer == projectile.owner && Main.rand.NextFloat() < 0.2f)
            {
                target.AddBuff(BuffID.OnFire, 480);
            }
        }

        public override void AI()
        {
            for (int x = 0; x < 3; x++)
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height, DustID.Fire, vX * 0.2f, vY * 0.2f, 100, Color.OrangeRed, 0.4f);
            AI0++;
            // Apply gravity and slow horizontal speed after half a second
            if (AI0 >= 30f)
            {
                if (vY < 9.8f)
                {
                    vY += 0.2f;
                }

                if (Math.Abs(vX) > 1)
                {
                    vX = Math.Abs(vX) - 0.1f;
                    vX *= projectile.direction;
                }
            }
        }
    }
}