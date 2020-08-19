using Desolation.NPCs.Librarian.Projectiles;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.NPCs.Librarian
{
    public class Librarian : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Librarian");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            // AI
            npc.aiStyle = -1;

            // Combat
            npc.lifeMax = 20000;
            npc.damage = 15;
            npc.defense = 12;
            npc.knockBackResist = 0f;
            npc.buffImmune[24] = true;

            // Aesthetics
            npc.width = 120;
            npc.height = 180;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            music = MusicID.Boss2;

            // Boss features
            npc.npcSlots = 15f;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;

            npc.value = Item.buyPrice(0, 10, 0, 0);
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter > 30)
            {
                npc.frameCounter = 0;
                int frame = 1 + (npc.frame.Y / frameHeight);
                if (frame >= Main.npcFrameCount[npc.type])
                {
                    frame = 0;
                }

                npc.frame.Y = frame * frameHeight;
            }
        }

        public float speed = 8.4f;

        #region AI Property Accessors

        public enum States
        {
            Initial = 0, Burst, CircleOfBooks
        }

        public States AI_State
        {
            get => (States)npc.ai[0];
            set => npc.ai[0] = (float)value;
        }

        public float AI_Timer
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        public int AI_2
        {
            get => (int)npc.ai[2];
            set => npc.ai[2] = value;
        }

        public int AI_3
        {
            get => (int)npc.ai[3];
            set => npc.ai[3] = value;
        }

        public Player Target => Main.player[npc.target];

        // Check if the target is alive and active
        public bool Target_Valid => Target.active && !Target.dead;

        #endregion AI Property Accessors

        public void Initialize()
        {
            AI_Timer = 0;
            AI_State = States.Burst;
        }

        #region Movements

        private bool g;

        // Move to the given position at maximum speed
        private void SimpleMove(Vector2 dest, int inertia = 20)
        {
            Vector2 direction = dest - npc.Center;
            direction.Normalize();
            direction *= speed;
            npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
        }

        #endregion Movements

        protected Vector2 burstCenter = new Vector2();

        public void Burst()
        {
            if (AI_Timer == 0 || !Target_Valid)
            {
                npc.TargetClosest(false);
                AI_Timer = 1;
            }
            AI_Timer++;
            Vector2 dest = Target.Center;
            dest.Y -= 480;
            dest.X += npc.Center.X < Target.Center.X ? -480 : 480;

            SimpleMove(dest);

            if (AI_Timer > 249 && AI_Timer < 421)
            {
                if (burstCenter == default(Vector2)) burstCenter = Target.Center;
                if (AI_Timer % 20 == 0)
                {
                    float angle = MathHelper.Pi / 18;
                    int pos = (int)Math.Floor(AI_Timer / 20);
                    Vector2 shoot = npc.Center - burstCenter;

                    shoot = shoot.RotatedBy((pos * angle));
                    shoot.Normalize();
                    shoot *= 10f;

                    Projectile.NewProjectile(npc.Center, shoot, ProjectileID.EyeLaser, npc.damage, 2);
                }
            }
            else
            {
                burstCenter = new Vector2();
            }

            if (AI_Timer > 441)
            {
                burstCenter = new Vector2();
                AI_Timer = 0;
                AI_State = States.CircleOfBooks;
            }
        }

        public void BookCircle()
        {
            if (AI_Timer == 0 || !Target_Valid)
            {
                npc.TargetClosest(false);
                AI_Timer = 1;
            }
            npc.velocity *= 0;
            if (AI_Timer % 20 == 0 && AI_Timer <= 360)
            {
                Vector2 projSpawn = npc.Center;
                projSpawn.Y -= 180;

                npc.TargetClosest();
                Projectile.NewProjectile(projSpawn, default, ProjectileType<CircleBook>(), npc.damage, 2.2f, 255, npc.target, npc.AngleTo(Target.Center));
                Main.PlaySound(SoundID.DD2_BookStaffCast, projSpawn);
            }

            if (AI_Timer > 720)
            {
                AI_Timer = 0;
                AI_State = States.Burst;
            }

            AI_Timer++;
        }

        public override void AI()
        {
            switch (AI_State)
            {
                case States.Initial:
                    Initialize();
                    break;

                case States.Burst:
                    Burst();
                    break;

                case States.CircleOfBooks:
                    BookCircle();
                    break;

                default:
                    Main.NewText("Invalid State");
                    break;
            }
        }
    }
}