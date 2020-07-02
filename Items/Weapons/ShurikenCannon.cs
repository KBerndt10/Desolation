using Desolation.Items.Ammo;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

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
            item.damage = 15;
            item.shootSpeed = 11.6f;
            item.useTime = 14;
            item.useAnimation = 14;
            item.knockBack = 1;

            // Cosmetic
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = SoundID.Item11;
            item.width = 17;
            item.height = 33;

            // Ammo
            item.useAmmo = ItemType<Shuriken>();
            item.shoot = 10;

            // Misc
            item.value = 10000;
            item.rare = ItemRarityID.Green;
        }
    }
}