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
            projectile.width = 36;
            projectile.height = 28;
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

            projectile.UpdateFrame();
            projectile.position.X = player.position.X + projectile.ai[0];
            projectile.position.Y = player.position.Y + projectile.ai[1];

            if (projectile.owner == Main.myPlayer)
                projectile.rotation = projectile.AngleTo(Main.MouseWorld);
        }
    }
}