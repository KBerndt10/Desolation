using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.NPCs.Oculus.Items
{
    internal class ChaoticEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It watches you.");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 9));
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            // Aesthetics
            item.width = 16;
            item.height = 16;
            item.rare = ItemRarityID.Red;

            // Utility
            item.maxStack = 20;
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
            return !NPC.AnyNPCs(NPCType<Oculus>()) && base.CanUseItem(player);
        }

        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, NPCType<Oculus>());
            Main.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}