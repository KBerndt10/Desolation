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
            item.damage = 30;
            item.ranged = true;
            item.noMelee = true;
            item.knockBack = 1;
            item.shootSpeed = 7.2f;
            item.useAmmo = AmmoID.Bullet;
            item.shoot = 10;

            // Use time
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.autoReuse = true;

            // Aesthetics
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item11;
            item.width = 30;
            item.height = 18;

            // Rarity
            item.value = 5400;
            item.rare = ItemRarityID.Orange;
        }

        private void SpawnTurrets(Player player)
        {
            if (RightTurret == -1 || Main.projectile[RightTurret].type != mod.ProjectileType("ShoulderTurret"))
            {
                RightTurret = Projectile.NewProjectile(player.position, new Vector2(0, 0), mod.ProjectileType("ShoulderTurret"), 0, 0, player.whoAmI, 6, -4);
            }

            if (LeftTurret == -1 || Main.projectile[LeftTurret].type != mod.ProjectileType("ShoulderTurret"))
            {
                LeftTurret = Projectile.NewProjectile(player.position, new Vector2(0, 0), mod.ProjectileType("ShoulderTurret"), 0, 0, player.whoAmI, -6, -4);
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

                Projectile.NewProjectile(Main.projectile[LeftTurret].position, left, type, damage, knockBack, player.whoAmI);
                Projectile.NewProjectile(Main.projectile[RightTurret].position, right, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            if (Desolation.DEBUG)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}