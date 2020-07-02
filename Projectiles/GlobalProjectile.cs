using System;
using Terraria;
using Terraria.ModLoader;

namespace Desolation.NPCs
{
    public class DesolationGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.minion)
            {
                DesolationPlayer player = Main.player[projectile.owner].GetModPlayer<DesolationPlayer>();
                if (player != null)
                {
                    Main.player[projectile.owner].statLife += (int)Math.Ceiling(damage * 0.05);
                }
            }
            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }
    }
}