using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Desolation.NPCs.Librarian
{
    internal class Phase2Book : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 64;
            npc.height = 64;

            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
        }

        protected enum States
        {
            Initial = 0
        }

        protected float AI_Timer
        {
            get => npc.ai[0];
            set => npc.ai[0] = value;
        }

        protected States AI_State
        {
            get => (States)npc.ai[1];
            set => npc.ai[1] = (float)value;
        }

        protected NPC Master => Main.npc[(int)npc.ai[2]];
        protected int Side => (int)npc.ai[3];

        public override void FindFrame(int frameHeight)
        {
            npc.frame.X = Side;
            if (AI_State != States.Initial)
            {
                npc.frame.Y = 0;
            }
            else
            {
                npc.frame.X = npc.width * Side;
                npc.frameCounter++;
                if (npc.frameCounter >= 30)
                {
                    npc.frameCounter = 0;
                    npc.frame.Y = npc.frame.Y == 0 ? frameHeight : 0;
                }
            }
        }

        private void SimpleMove(Vector2 dest, float speed = 8f, int inertia = 20)
        {
            Vector2 direction = dest - npc.Center;
            direction.Normalize();
            direction *= speed;
            npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
        }

        public void SetPosition()
        {
        }

        public override void AI()
        {
            switch (AI_State)
            {
                case States.Initial:
                    Vector2 dest = Master.Center;
                    dest.X += Side == 0 ? -48 : 48;
                    SimpleMove(dest);
                    break;
            }
        }
    }
}