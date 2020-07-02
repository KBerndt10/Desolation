using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.Items.Armor.Necromancer
{
    [AutoloadEquip(EquipType.Head)]
    public class NecromancerHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The hood of a mighty necromancer");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 750;
            item.rare = ItemRarityID.Blue;
            item.defense = 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ItemType<NecromancerRobe>() && legs.type == ItemType<NecromancerPants>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+1 max minions.";
            player.maxMinions++;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RottenFlesh"), 25);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}