using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Weapons
{
    public class ShoulderCannon : ModItem
    {
        private int LeftTurret = -1;
        private int RightTurret = -1;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fire bullets from your convenient shoulder cannons");
        }

        public override void SetDefaults()
        {
            // Combat
            item.damage = 36;
            item.ranged = true;
            item.noMelee = true;
            item.knockBack = 2;
            item.shootSpeed = 14f;
            item.useAmmo = AmmoID.Bullet;
            item.shoot = 10;

            // Use time
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;

            // Aesthetics
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item11;
            item.width = 30;
            item.height = 18;

            // Rarity
            item.value = 40000;
            item.rare = ItemRarityID.LightRed;
        }

        private void SpawnTurrets(Player player)
        {
            if (RightTurret == -1 || Main.projectile[RightTurret].type != mod.ProjectileType("ShoulderTurret"))
            {
                RightTurret = Projectile.NewProjectile(player.position, new Vector2(0, 0), mod.ProjectileType("ShoulderTurret"), 0, 0, player.whoAmI, 24, -16);
            }

            if (LeftTurret == -1 || Main.projectile[LeftTurret].type != mod.ProjectileType("ShoulderTurret"))
            {
                LeftTurret = Projectile.NewProjectile(player.position, new Vector2(0, 0), mod.ProjectileType("ShoulderTurret"), 0, 0, player.whoAmI, -42, -16);
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (player.inventory[player.selectedItem].type == item.type)
            {
                SpawnTurrets(player);
            }
            else
            {
                LeftTurret = RightTurret = -1;
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                float power = new Vector2(speedX, speedY).Length();
                Vector2 left = (Main.MouseWorld - Main.projectile[LeftTurret].position);
                Vector2 right = (Main.MouseWorld - Main.projectile[RightTurret].position);
                left.Normalize();
                right.Normalize();
                left *= power;
                right *= power;
                //left = left.RotatedBy(Main.projectile[LeftTurret].rotation);
                // right = right.RotatedBy(Main.projectile[RightTurret].rotation);

                Projectile.NewProjectile(Main.projectile[LeftTurret].Center, left, type, damage, knockBack, player.whoAmI);
                Projectile.NewProjectile(Main.projectile[RightTurret].Center, right, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.AddIngredient(ItemID.HallowedBar, 12);
            recipe.AddIngredient(ItemID.SoulofFright, 16);
            recipe.AddIngredient(ItemID.IllegalGunParts, 2);
            recipe.AddRecipe();
        }
    }
}