using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Desolation.Items
{
    internal class DesolationGlobalItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Shuriken)
            {
                item.ammo = item.type;
            }
        }
    }
}