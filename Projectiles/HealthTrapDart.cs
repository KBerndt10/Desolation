using Desolation.Utility;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Projectiles
{
    public class HealthTrapDart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Dart");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.timeLeft = 600;
            projectile.hostile = true;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.damage = 0;
            projectile.knockBack = 0;
            projectile.penetrate = 1;
            aiType = ProjectileID.BoneArrow;
        }

        private void Heal(NPC npc)
        {
            if (npc.life < npc.lifeMax)
            {
                int heal = Math.Min(npc.lifeMax - npc.life, 35);
                npc.HealEffect(heal);
                npc.life += heal;
            }
        }

        private void Heal(Player player)
        {
            if (player.statLife < player.statLifeMax2)
            {
                int heal = Math.Min(player.statLifeMax2 - player.statLife, 35);
                player.HealEffect(heal);
                player.statLife += heal;
            }
        }

        public override void Kill(int timeLeft)
        {
            foreach (int i in Util.NearbyPlayers(projectile.position, 80))
            {
                Heal(Main.player[i]);
            }
            foreach (int i in Util.NearbyNPCs(projectile.position, 80))
            {
                Heal(Main.npc[i]);
            }
        }
    }
}