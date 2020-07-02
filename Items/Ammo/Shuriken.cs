using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Ammo
{
    public class Shuriken : ModItem
    {
        public override void SetDefaults()
        {
            // Combat
            item.damage = 10;
            item.ranged = true;
            item.knockBack = 0;
            item.useTime = 14;
            item.useTime = 14;
            item.thrown = true;
            item.noMelee = true;
            item.shootSpeed = 7.2f;

            // Ammo
            item.shoot = ProjectileID.Shuriken;
            item.ammo = item.type;

            // Cosmetic
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 11;
            item.height = 11;

            //Misc
            item.maxStack = 999;
            item.consumable = true;
            item.value = Item.sellPrice(0, 0, 0, 3);
            item.rare = ItemRarityID.White;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Shuriken);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Shuriken, 10);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.SetResult(ItemID.Shuriken);
            recipe.AddRecipe();
        }
    }
}