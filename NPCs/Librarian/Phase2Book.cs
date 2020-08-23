using Desolation.NPCs.Librarian.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.NPCs.Librarian
{
    internal class Phase2Book : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("");
            Main.npcFrameCount[npc.type] = 4;
        }

        protected int flameDamage = 32;
        protected int waterDamage = 24;

        public override void SetDefaults()
        {
            npc.width = 64;
            npc.height = 64;

            npc.damage = 0;

            npc.lifeMax = int.MaxValue;

            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            if (Main.expertMode)
            {
                flameDamage *= 2;
                waterDamage = (int)(waterDamage * 1.5);
            }
        }

        protected enum States
        {
            Initial = 0, Elemental, Neutral
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

        protected int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            if (AI_State != States.Initial)
            {
                frame = 0;
            }
            else
            {
                npc.frameCounter++;
                if (npc.frameCounter > 20)
                {
                    frame++;
                    if (frame > 1) frame = 0;
                    npc.frameCounter = 0;
                }
            }

            npc.frame.Y = (frame * frameHeight) + (2 * Side * frameHeight);
        }

        private void SimpleMove(Vector2 dest, float speed = 8f, int inertia = 20)
        {
            Vector2 direction = dest - npc.Center;
            direction.Normalize();
            direction *= speed;
            npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
        }

        protected void StickToMaster()
        {
            Vector2 dest = Master.Center;
            dest.X += Side == 0 ? -104 : 104;
            SimpleMove(dest);
        }

        public void InitialState()
        {
            StickToMaster();
            if (++AI_Timer > 600)
            {
                AI_Timer = 0;
                AI_State = States.Neutral;
            }
        }

        protected void Fire()
        {
            StickToMaster();
            if (AI_Timer % 20 == 0)
            {
                int limit = Main.expertMode ? 15 : 6;
                for (int i = 0; i < limit; i++)
                {
                    float angle = (MathHelper.TwoPi / limit) * i;
                    Vector2 shoot = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    shoot.Normalize();
                    shoot *= 10.4f;

                    Projectile.NewProjectile(npc.Center, shoot, ProjectileID.Fireball, flameDamage, 2.2f);
                }
            }
        }

        protected void Water()
        {
            StickToMaster();
            if (Main.netMode != NetmodeID.MultiplayerClient && AI_Timer % 6 == 0)
            {
                Vector2[] aim = { new Vector2(-6, -6), new Vector2(6, -6), new Vector2(6, 6), new Vector2(-6, 6) };

                for (int i = 0; i < aim.Length; i++)
                {
                    Projectile.NewProjectile(npc.Center, aim[i], ProjectileType<BookWater>(), waterDamage, 0.25f);
                }
            }
        }

        public override void AI()
        {
            if (Master.type != NPCType<Librarian>() || !Master.active)
            {
                npc.life = -1;
            }
            else
            {
                npc.timeLeft = 10;
            }

            switch (AI_State)
            {
                case States.Initial:
                    InitialState();
                    break;

                case States.Neutral:
                    StickToMaster();
                    AI_Timer++;
                    if (AI_Timer > (Main.expertMode ? 300 : 640))
                    {
                        AI_Timer = 0;
                        AI_State = States.Elemental;
                    }
                    break;

                case States.Elemental:
                    if (Side == 0) Water();
                    else Fire();
                    AI_Timer++;
                    if (AI_Timer > (300))
                    {
                        AI_State = States.Neutral;
                        AI_Timer = 0;
                    }
                    break;
            }
        }
    }
}