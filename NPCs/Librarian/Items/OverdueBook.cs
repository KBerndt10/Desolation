using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.NPCs.Librarian.Items
{
    internal class OverdueBook : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overdue Book");
            Tooltip.SetDefault("Be ready to pay the late fine...");
        }

        public override void SetDefaults()
        {
            // Aesthetics
            item.width = 16;
            item.height = 16;
            item.rare = ItemRarityID.Red;

            // Utility
            item.maxStack = Desolation.DEBUG ? 999 : 20;
            item.value = Item.buyPrice(0, 1, 0, 0);

            // Usage
            item.consumable = true;
            item.useAnimation = 40;
            item.useTime = 40;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.UseSound = SoundID.Item44;
        }

        // TODO: Implement conditions
        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(NPCType<Librarian>()) && base.CanUseItem(player);
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, NPCType<Librarian>());
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}