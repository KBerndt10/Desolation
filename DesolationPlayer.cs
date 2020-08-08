using Desolation.Items.Fish;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation
{
    // ModPlayer classes provide a way to attach data to Players and act on that data. ExamplePlayer has a lot of functionality related to
    // several effects and items in ExampleMod. See SimpleModPlayer for a very simple example of how ModPlayer classes work.
    public class DesolationPlayer : ModPlayer
    {
        public bool necroPact = false;

        public override void ResetEffects()
        {
            necroPact = false;
        }

        public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
        {
            if (junk)
            {
                return;
            }
            if (Desolation.DEBUG && liquidType == 0 && player.ZoneBeach && Main.rand.Next(0, 3) == 1)
            {
                caughtType = ItemType<BlueTang>();
            }
        }
    }
}