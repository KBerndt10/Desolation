using Desolation.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.NPCs.Oculus.Projectiles
{
    internal class BouncyEye : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bouncy Eye");
            Main.projFrames[projectile.type] = 9;
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Fireball);
            projectile.width = 16;
            projectile.height = 12;
            projectile.aiStyle = -1;
            projectile.light = 0.5f;
        }

        // Adapted From Vanilla Ai Style 8 (Golem's fireball, projectile #258)
        public override void AI()
        {
            if (projectile.localAI[0] == 0f) // Spawning Sound
            {
                projectile.localAI[0] = 1f;
                Main.PlaySound(SoundID.Item20, projectile.Center);
            }
            else
            {
                // Make 2 dusts
                for (int i = 0; i < 1; i++)
                {
                    int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, Color.Black, 2f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity.X *= 0.3f;
                    Main.dust[dust].velocity.Y *= 0.3f;
                }
            }

            // Start falling after 3/4 sec
            if (projectile.ai[1] >= 45f)
            {
                projectile.velocity.Y += 0.2f;
            }
            else
            {
                // rotate towards where it is going
                projectile.rotation += 0.3f * projectile.direction;
            }

            // Cap the falling rate
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            projectile.UpdateFrame();
        }
    }
}