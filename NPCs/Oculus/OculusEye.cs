using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.NPCs.Oculus
{
    internal abstract class OculusEye : ModNPC
    {
        public bool attached = true;
        protected bool LookAtPlayer = true;
        protected int frame;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 14;
        }

        public override void SetDefaults()
        {
            npc.value = 5000f;

            // Cosmetic
            animationType = -1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.alpha = 0;

            // AI
            npc.aiStyle = -1;
            aiType = -1;

            // Combat
            npc.knockBackResist = 0f;

            npc.noGravity = true;
            npc.noTileCollide = true;
        }

        // Which slot in the npc.ai[] is which

        protected const int AI_Master_Slot = 0;
        protected const int AI_State_Slot = 1;
        protected const int AI_Timer_Slot = 2;
        protected const int AI_Slot_3 = 3;

        // State values
        protected enum State
        {
            Waiting, Detaching, Pursue
        }

        protected const float speed = 8f;
        protected const float acceleration = 1.2f;

        // Getters and Setters for AI slots for convenience
        public int AI_Master
        {
            get => (int)npc.ai[AI_Master_Slot];
            set => npc.ai[AI_Master_Slot] = value;
        }

        protected State AI_State
        {
            get => (State)npc.ai[AI_State_Slot];
            set => npc.ai[AI_State_Slot] = (float)value;
        }

        public float AI_Timer
        {
            get => npc.ai[AI_Timer_Slot];
            set => npc.ai[AI_Timer_Slot] = value;
        }

        public float AI_3
        {
            get => npc.ai[AI_Slot_3];
            set => npc.ai[AI_Slot_3] = value;
        }

        public Player Target
        {
            get => Main.player[npc.target];
            set => npc.target = value.whoAmI;
        }

        public float Speed
        {
            get => npc.velocity.Length();
            set { npc.velocity.Normalize(); npc.velocity *= value; }
        }

        public NPC Master => Main.npc[AI_Master];
        public Oculus.State MasterState => (Oculus.State)Master.ai[0];
        public int MasterTimer => (int)Master.ai[1];

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter > 13)
            {
                npc.frameCounter = 0;
                if (++frame > 6)
                {
                    frame = 0;
                }
            }
            npc.frame.Y = frame * frameHeight;
            if (LookAtPlayer)
            {
                npc.rotation = npc.AngleTo(Target.Center) + MathHelper.PiOver4;
            }
            base.FindFrame(frameHeight);
        }

        protected void Wait()
        {
            AI_Timer -= 2;
            if (AI_Timer <= 0)
            {
                AI_State = State.Pursue;
                AI_Timer = 0;
            }
            else
            {
                npc.velocity *= 0.99f;
            }
        }

        public bool StayAttached()
        {
            return npc.life > npc.lifeMax / 2;
        }

        public abstract void Detach();

        public abstract void StickToMaster(Vector2 center);

        public void Cry()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && MasterTimer % 5 == 0)
            {
                Projectile.NewProjectile(npc.Center.X + Main.rand.NextFloat(npc.width / 2, npc.width / -2), npc.position.Y + npc.height, 0, 4, ProjectileID.RainNimbus, (int)Math.Ceiling(npc.damage * .66f), 1.2f);
            }
        }

        public override void AI()
        {
            // Get rid of this is the master is gone
            if (!Master.active || Master.type != NPCType<Oculus>())
            {
                npc.active = false;
            }
            else
            {
                npc.timeLeft = 10;
            }

            if (attached)
            {
                if (StayAttached() && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    npc.TargetClosest();
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        switch (MasterState)
                        {
                            case Oculus.State.Crying:
                                Cry();
                                break;
                        }
                    }
                }
                else
                {
                    attached = false;
                    Detach();
                }
            }
            else
            {
                // TODO: Unattached AI
            }
        }
    }
}