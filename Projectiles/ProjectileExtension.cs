using Terraria;

namespace Desolation.Projectiles
{
    public static class ProjectileExtension
    {
        public static void UpdateFrame(this Projectile projectile, int frameDelay = 10)
        {
            bool b = false;
            projectile.UpdateFrame(ref b, frameDelay);
        }

        public static void UpdateFrame(this Projectile projectile, ref bool swapDir, int frameDelay = 10)
        {
            if (++projectile.frameCounter > frameDelay)
            {
                projectile.frameCounter = 0;

                projectile.frame += swapDir ? -1 : 1;
                swapDir = !swapDir;

                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
    }
}