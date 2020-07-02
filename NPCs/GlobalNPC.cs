using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.NPCs
{
    public class DesolationGlobalNPC : GlobalNPC
    {
        private static int[] zombies = { NPCID.ArmedZombie, NPCID.ArmedZombieCenx, NPCID.ArmedZombieEskimo, NPCID.ArmedZombiePincussion, NPCID.ArmedZombieSlimed, NPCID.ArmedZombieSwamp, NPCID.ArmedZombieTwiggy,
            NPCID.BaldZombie, NPCID.BigBaldZombie, NPCID.BigFemaleZombie, NPCID.BigPincushionZombie, NPCID.BigRainZombie, NPCID.BigSlimedZombie, NPCID.BigSwampZombie, NPCID.BigTwiggyZombie, NPCID.BigZombie,
            NPCID.BloodZombie, NPCID.FemaleZombie, NPCID.PincushionZombie, NPCID.SlimedZombie, NPCID.SmallBaldZombie, NPCID.SmallFemaleZombie, NPCID.SmallPincushionZombie, NPCID.SmallRainZombie, NPCID.SmallSlimedZombie,
            NPCID.SmallSwampZombie, NPCID.SmallTwiggyZombie, NPCID.Zombie, NPCID.ZombieDoctor,NPCID.ZombieEskimo, NPCID.ZombieMushroom, NPCID.ZombieMushroomHat, NPCID.ZombiePixie, NPCID.ZombieRaincoat, NPCID.ZombieSuperman,
            NPCID.ZombieSweater, NPCID.ZombieXmas
        };

        public override void NPCLoot(NPC npc)
        {
            if (zombies.Contains(npc.netID))
            {
                Item.NewItem(npc.getRect(), mod.ItemType("RottenFlesh"), Main.rand.Next(1, 3));
            }
        }
    }
}