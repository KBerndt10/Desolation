using Desolation.Buffs;
using Desolation.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.Items.Weapons
{
    internal class ZombieHeadStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Necromancer's Staff");
            Tooltip.SetDefault("Summons a flying zombie head to fight for you.");
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            // Combat
            item.damage = 9;
            item.knockBack = 1.5f;
            item.mana = 1;
            item.noMelee = true;

            // Minion
            item.summon = true;
            item.buffType = BuffType<ZombieHeadBuff>();
            item.shoot = ProjectileType<ZombieHead>();

            // Cosmetic
            item.width = 32;
            item.height = 32;
            item.UseSound = SoundID.Item44;

            //Usage
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;

            item.value = Item.buyPrice(0, 0, 75, 0);
            item.rare = ItemRarityID.Blue;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2);
            position = Main.MouseWorld;
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RottenFlesh"), 20);
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}