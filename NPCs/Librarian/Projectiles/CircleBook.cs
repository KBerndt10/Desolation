using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.NPCs.Librarian.Projectiles
{
    public class CircleBook : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Forbidden Knowledge");
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 840;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (circle)
            {
                projectile.frame = 0;
            }
            else
            {
                projectile.frame = 1;
                projectile.rotation += projectile.direction * 0.03f;
            }
            return base.PreDraw(spriteBatch, lightColor);
        }

        protected float AI_0
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }

        protected float AI_1
        {
            get => projectile.ai[1];
            set => projectile.ai[1] = value;
        }

        protected bool circle = true;

        public override void AI()
        {
            if (!circle) return;

            // Initilialize
            if (projectile.timeLeft == 840)
            {
                Vector2 target = Main.player[(int)AI_0].Center;
                AI_1 = 0;
            }

            int upTime = 840 - projectile.timeLeft;

            //Circle
            if (upTime < 360)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float rads = MathHelper.ToRadians(upTime);
                    Vector2 dest = new Vector2(projectile.position.X + 3 * (float)Math.Cos(rads), projectile.position.Y + 3 * (float)Math.Sin(rads));
                    projectile.velocity = dest - projectile.position;
                    projectile.netUpdate = true;
                }
            }
            else // Shoot
            {
                Player target = Main.player[(int)AI_0];
                if (target.active)
                {
                    Vector2 toTarget = target.Center - projectile.Center;
                    toTarget.Normalize();
                    toTarget *= 9.6f;
                    projectile.velocity = toTarget;
                }
                else
                {
                    Vector2 shoot = (new Vector2(0, -1)).RotatedBy(AI_1);
                    shoot *= 9.6f;
                    projectile.velocity = shoot;
                }
                Main.PlaySound(SoundID.NPCHit50, projectile.Center);
                circle = false;
            }
        }
    }
}