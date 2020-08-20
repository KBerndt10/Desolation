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

        public int CircleBookDamage = 28;
        public int burstDamage = 36;

        public override void SetDefaults()
        {
            // AI
            npc.aiStyle = -1;

            // Combat
            npc.lifeMax = 6000;
            npc.damage = 36;
            npc.defense = 10;
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

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            // Expert mode scaling
            if (Main.expertMode)
            {
                npc.lifeMax = (int)(1.5 * npc.lifeMax);
                npc.defense *= 2;
                npc.damage *= 3;
                burstDamage = (int)(burstDamage * 1.5);
                CircleBookDamage = (int)(CircleBookDamage * 1.5);
            }

            // player count scaling
            npc.lifeMax *= (int)((Main.ActivePlayersCount - 1) * 1.2);

            // ensure that the boss starts at full health
            npc.life = npc.lifeMax;
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
            Initial = 0, Burst, CircleOfBooks, Phase2
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

        protected bool Phase2 = false;

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, damage);
            if (!Phase2 && npc.life < npc.lifeMax * 0.6 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Phase2 = true;
                AI_State = States.Phase2;
                AI_Timer = 0;
            }
        }

        #region Movements

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

                    Projectile.NewProjectile(npc.Center, shoot, ProjectileID.EyeLaser, burstDamage, 2);
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
            if (AI_Timer % 20 == 0 && AI_Timer <= 360 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 projSpawn = npc.Center;
                projSpawn.Y -= 180;

                npc.TargetClosest();
                Projectile.NewProjectile(projSpawn, default, ProjectileType<CircleBook>(), CircleBookDamage, 2.2f, 255, npc.target, npc.AngleTo(Target.Center));
                Main.PlaySound(SoundID.DD2_BookStaffCast, projSpawn);
            }

            if (AI_Timer > 720)
            {
                AI_Timer = 0;
                AI_State = States.Burst;
            }

            AI_Timer++;
        }

        protected bool madeBooks = false;

        public void Phase2Transition()
        {
            Phase2 = true;
            if (!madeBooks && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<Phase2Book>(), 0, 0, 0, npc.whoAmI, 0);
                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCType<Phase2Book>(), 0, 0, 0, npc.whoAmI, 1);
            }
            madeBooks = true;
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

                case States.Phase2:
                    Phase2Transition();
                    break;

                default:
                    Main.NewText("Invalid State");
                    break;
            }
        }
    }
}