using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Weapons
{
    public class Zombow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Armed & Dangerous");
        }

        public override void SetDefaults()
        {
            item.damage = 15;
            item.ranged = true;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.UseSound = SoundID.Item11;
            item.shootSpeed = 8.4f;
            item.width = 17;
            item.height = 33;
            item.value = 10000;
            item.useAmmo = AmmoID.Arrow;
            item.rare = ItemRarityID.Green;
            item.shoot = 10;
            item.autoReuse = true;
            item.UseSound = SoundID.Item11;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RottenFlesh"), 25);
            recipe.AddIngredient(ItemID.ZombieArm, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}