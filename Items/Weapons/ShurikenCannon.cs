using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Weapons
{
    public class ShurikenCannon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots Shurikens at a rapid rate");
        }

        public override void SetDefaults()
        {
            // Combat Power
            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.damage = 16;
            item.shootSpeed = 11.6f;
            item.useTime = 14;
            item.useAnimation = 14;
            item.knockBack = 1;

            // Cosmetic
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item11;
            item.width = 27;
            item.height = 12;

            // Ammo
            item.useAmmo = ItemID.Shuriken;
            item.shoot = 10;

            // Misc
            item.value = 10000;
            item.rare = ItemRarityID.Green;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return !(player.head == ItemID.NinjaHood && player.body == ItemID.NinjaShirt && player.legs == ItemID.NinjaPants) && base.ConsumeAmmo(player);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddTile(TileID.Anvils);
            recipe.AddIngredient(ItemID.IllegalGunParts);
            recipe.AddIngredient(ItemID.Shuriken, 50);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}