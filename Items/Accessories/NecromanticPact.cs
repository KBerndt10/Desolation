using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Accessories
{
    public class NecromanticPact : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A powerful relic.\n Your melee minions heal you for 5% of the damage they deal\n You can never have more than 75% health");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.accessory = true;
            item.value = 50000;
            item.rare = ItemRarityID.Orange;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DesolationPlayer>().necroPact = true;
            if (player.statLife > player.statLifeMax2 * 0.75) player.statLife = (int)(player.statLifeMax2 * 0.75);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RottenFlesh"), 150);
            recipe.AddIngredient(ItemID.Shackle);
            recipe.AddIngredient(ItemID.Bunny);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}