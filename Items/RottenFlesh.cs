using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items
{
    public class RottenFlesh : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Tasty?");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 20;
            item.maxStack = 999;
            item.value = 50;
            item.rare = ItemRarityID.White;
        }
    }
}