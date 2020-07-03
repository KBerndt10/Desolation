using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.Items.Placeable
{
    public class HealthTrap : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.maxStack = 99;
            item.consumable = true;
            item.createTile = TileType<Tiles.HealthTrap>();
            item.width = 12;
            item.height = 12;
            item.value = 25000;
            item.mech = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DartTrap);
            recipe.AddIngredient(ItemID.GreaterHealingPotion, 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}