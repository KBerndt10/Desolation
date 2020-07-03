using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Desolation.Tiles
{
    // This class shows off a number of less common ModTile methods. These methods help our trap tile behave like vanilla traps.
    // In particular, hammer behavior is particularly tricky. The logic here is setup for multiple styles as well.
    public class HealthTrap : ModTile
    {
        public override void SetDefaults()
        {
            TileID.Sets.DrawsWalls[Type] = true;

            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileFrameImportant[Type] = true;

            // These 2 AddMapEntry and GetMapOption show off multiple Map Entries per Tile. Delete GetMapOption and all but 1 of these for your own ModTile if you don't actually need it.
            AddMapEntry(new Color(21, 179, 192), Language.GetText("MapObject.Trap")); // localized text for "Trap"
            AddMapEntry(new Color(0, 141, 63), Language.GetText("MapObject.Trap"));
        }

        // Read the comments above on AddMapEntry.
        public override ushort GetMapOption(int i, int j) => (ushort)(Main.tile[i, j].frameY / 18);

        public override bool Dangersense(int i, int j, Player player) => true;

        public override bool Drop(int i, int j)
        {
            Tile t = Main.tile[i, j];
            Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("HealthTrap"));
            return base.Drop(i, j);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            type = 13; // A blue dust to match the tile TODO
            return true;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            Tile tile = Main.tile[i, j];
            if (Main.LocalPlayer.direction == 1)
            {
                tile.frameX += 18;
            }
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1, TileChangeType.None);
            }
        }

        private static int[] frameXCycle = { 2, 3, 4, 5, 1, 0 };

        // Hammerinf rotates the tile
        public override bool Slope(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int nextFrameX = frameXCycle[tile.frameX / 18];
            tile.frameX = (short)(nextFrameX * 18);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1, TileChangeType.None);
            }
            return false;
        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            Vector2 spawnPosition;
            // Calculate shooting angle based on orientation
            int horizontalDirection = (tile.frameX == 0) ? -1 : ((tile.frameX == 18) ? 1 : 0);
            int verticalDirection = (tile.frameX < 36) ? 0 : ((tile.frameX < 72) ? -1 : 1);

            if (Wiring.CheckMech(i, j, 60))
            {
                spawnPosition = new Vector2(i * 16 + 8 + 0 * horizontalDirection, j * 16 + 9 + 0 * verticalDirection); // The extra numbers here help center the projectile spawn position if you need to.
                Projectile.NewProjectile(spawnPosition, new Vector2(horizontalDirection, verticalDirection) * 6f, mod.ProjectileType("HealthTrapDart"), 10, 1.2f, Main.myPlayer);
            }
        }
    }
}