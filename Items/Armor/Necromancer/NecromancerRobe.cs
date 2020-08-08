using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items.Armor.Necromancer
{
    [AutoloadEquip(EquipType.Body)]
    internal class NecromancerRobe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A necromancer's robe");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 14;
            item.rare = ItemRarityID.Blue;
            item.defense = 1;
            item.value = 1000;
        }

        public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
        {
            robes = true;
            // The equipSlot is added in ExampleMod.cs --> Load hook
            equipSlot = mod.GetEquipSlot("NecromancerRobe_Legs", EquipType.Legs);
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RottenFlesh"), 50);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
};