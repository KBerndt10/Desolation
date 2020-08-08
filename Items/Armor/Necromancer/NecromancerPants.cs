using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Armor.Necromancer
{
    [AutoloadEquip(EquipType.Legs)]
    public class NecromancerPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A necromancer's pants");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 1000;
            item.rare = ItemRarityID.Blue;
            item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            //player.moveSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RottenFlesh"), 30);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}