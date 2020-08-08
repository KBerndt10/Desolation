using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Projectiles
{
    public class InfernoBolt : ModProjectile
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
            projectile.light = 0.8f;
        }

        private float pX
        {
            get => projectile.position.X;
            set => projectile.position.X = value;
        }

        private float pY
        {
            get => projectile.position.Y;
            set => projectile.position.Y = value;
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

        public override void AI()
        {
            for (int x = 0; x < 3; x++)
                Dust.NewDust(new Vector2(pX, pY + 2f), projectile.width, projectile.height, DustID.Vortex, vX * 0.2f, vY * 0.2f, 100, Color.OrangeRed, 0.4f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 480);
        }

        public override void Kill(int timeLeft)
        {
            projectile.light = 5f;
            Vector2 velocity = -projectile.velocity * 1.2f;
            double inc = MathHelper.Pi / 10;
            double endpoint = inc * 2;
            for (double i = -endpoint; i <= endpoint; i += inc)
                Projectile.NewProjectile(projectile.position, velocity.RotatedBy(i), mod.ProjectileType("InfernoBoltSpark"), projectile.damage / 2, 0, Main.myPlayer);
        }
    }
}