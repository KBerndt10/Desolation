using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Weapons
{
    public class Titanashark : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("66% chance not to use ammo.\nMegashark's Daddy");
        }

        public override void SetDefaults()
        {
            // Combat Power
            item.ranged = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.damage = 91;
            item.shootSpeed = 12.4f;
            item.useTime = 5;
            item.useAnimation = 5;
            item.knockBack = 2f;
            item.crit = 6;

            // Cosmetic
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item11;
            item.width = 40;
            item.height = 16;

            // Ammo
            item.useAmmo = AmmoID.Bullet;
            item.shoot = 10;

            // Misc
            item.value = 10000;
            item.rare = ItemRarityID.Green;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() < 0.33 && base.ConsumeAmmo(player);
        }

        public override void AddRecipes()
        {
            if (Desolation.DEBUG)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddTile(240);
                recipe.AddIngredient(ItemID.Megashark);
                recipe.AddIngredient(ItemID.IllegalGunParts, 20);
                recipe.AddIngredient(ItemID.LunarBar, 13);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}