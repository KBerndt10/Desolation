/*
 WIP, need to fix animated weapon first
 */

using Terraria;

namespace Desolation.Projectiles
{
    public class InfernoStaffRod : AnimatedWeapon
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.width = 17;
            projectile.height = 33;
            Main.projFrames[projectile.type] = 5;
            Proj = mod.ProjectileType("InfernoBolt");
        }
    }
}