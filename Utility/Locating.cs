using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;

namespace Desolation.Utility
{
    public static partial class Util
    {
        public static List<int> NearbyPlayers(Vector2 pos, float dist)
        {
            List<int> nearby = new List<int>();
            foreach (Player p in Main.player)
            {
                if (p != null && p.active && Util.dist(pos, p.position) <= dist)
                {
                    nearby.Add(p.whoAmI);
                }
            }

            return nearby;
        }

        public static List<int> NearbyNPCs(Vector2 pos, float dist)
        {
            List<int> nearby = new List<int>();
            foreach (NPC n in Main.npc)
            {
                if (n != null && n.active && Util.dist(pos, n.position) <= dist)
                {
                    nearby.Add(n.whoAmI);
                }
            }

            return nearby;
        }
    }
}