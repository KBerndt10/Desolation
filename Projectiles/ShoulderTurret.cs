using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Desolation.Projectiles
{
    public class ShoulderTurret : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shoulder Turret");
            Main.projFrames[projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 18;
            projectile.timeLeft = 999999;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.friendly = true;
            projectile.light = 0.8f;
        }

        private Player player => Main.player[projectile.owner];

        public override void AI()
        {
            if (player.inventory[player.selectedItem].type != mod.ItemType("ShoulderCannon"))
            {
                projectile.Kill();
            }
            projectile.position.X = player.position.X + projectile.ai[0] * 4;
            projectile.position.Y = player.position.Y + projectile.ai[1] * 4;

            //Vector2 mouse = Main.MouseWorld;
            Vector2 toMouse = Main.MouseWorld - projectile.position;
            projectile.spriteDirection = Main.MouseWorld.X < projectile.position.X ? 1 : -1;

            projectile.rotation = (float)Math.Atan2(toMouse.X, toMouse.Y);
        }
    }
}