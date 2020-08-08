using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Weapons
{
    public class InfernoStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Burn your foes with a spectacular inferno.");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.damage = 26;
            item.magic = true;
            item.mana = 11;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 1;
            item.UseSound = SoundID.Item11;
            item.shootSpeed = 7.2f;
            item.width = 25;
            item.height = 25;
            item.value = 5400;
            item.rare = ItemRarityID.Orange;
            item.shoot = mod.ProjectileType("InfernoBolt");
            item.autoReuse = true;
            item.UseSound = SoundID.Item11;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.Anvils);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}