using Desolation.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.Buffs
{
    internal class ZombieHeadBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Zombie Head");
            Description.SetDefault("A flying zombie head will fight for you.");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ProjectileType<ZombieHead>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
}