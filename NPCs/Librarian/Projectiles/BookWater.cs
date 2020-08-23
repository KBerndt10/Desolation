using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.NPCs.Librarian.Projectiles
{
    public class BookWater : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.penetrate = 5;
            projectile.extraUpdates = 2;
            projectile.ignoreWater = true;
            projectile.light = 0.5f;
        }

        public override void AI()
        {
            projectile.scale -= 0.002f;
            if (projectile.scale <= 0f)
            {
                projectile.Kill();
            }

            if (projectile.ai[0] > 3f)
            {
                projectile.velocity.Y += 0.075f;

                // Spawn Dust
                Vector2 pos = new Vector2(projectile.position.X + 14, projectile.position.Y + 14);
                for (int i = 0; i < 2; i++)
                {
                    int dustId = Dust.NewDust(pos, projectile.width - 14 * 2, projectile.height - 14 * 2, DustID.Ice, 0f, 0f, 100, Color.AliceBlue);
                    Main.dust[dustId].noGravity = true;
                    Dust dust = Main.dust[dustId];
                    dust.velocity *= 0.1f;
                    dust = Main.dust[dustId];
                    dust.velocity += projectile.velocity * 0.5f;

                    float xOffSet = projectile.velocity.X / 3f * i;
                    float YOffSet = projectile.velocity.Y / 3f * i;
                    Main.dust[dustId].position.X -= xOffSet;
                    Main.dust[dustId].position.Y -= YOffSet;
                }
                if (Main.rand.Next(8) == 0)
                {
                    int dustId = Dust.NewDust(new Vector2(projectile.position.X + 16, projectile.position.Y + 16), projectile.width - 16 * 2, projectile.height - 16 * 2, DustID.Ice, 0f, 0f, 100, Color.Blue, 0.5f);
                    Dust dust = Main.dust[dustId];
                    dust.velocity *= 0.25f;
                    dust.velocity += projectile.velocity * 0.5f;
                }
            }
            else
            {
                projectile.ai[0]++;
            }
        }
    }
}