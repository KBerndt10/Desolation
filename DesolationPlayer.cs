using Terraria.ModLoader;

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
    }
}