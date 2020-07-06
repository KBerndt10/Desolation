using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Fish
{
    public class BlueTang : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Tries to help her frind find his son.");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.maxStack = 999;
            item.value = 500;
            item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(ItemID.Sashimi);
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddIngredient(mod.ItemType("BlueTang"));
            recipe.AddRecipe();
        }
    }
}