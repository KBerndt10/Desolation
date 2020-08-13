﻿using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.NPCs.Oculus
{
    [AutoloadBossHead]
    internal class Oculus : ModNPC
    {
        protected int myFrame = 0;
        protected bool hasEyes = true;
        protected OculusEye[] Eyes = null;

        public override void SetStaticDefaults()
        {
            //Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.value = 5000f;

            // Cosmetic
            npc.width = 194;
            npc.height = 128;
            animationType = -1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.alpha = 0;

            // AI
            npc.aiStyle = -1;
            aiType = -1;
            npc.boss = true;

            // Combat
            npc.damage = 0;
            npc.defense = 25;
            npc.lifeMax = 35000;
            npc.knockBackResist = 0f;

            npc.noGravity = true;
            npc.noTileCollide = true;
            // Keep the body untargettable and health bar-free.
            npc.dontTakeDamage = true;
        }

        // Which state in the npc.ai[] is which

        private const int AI_State_Slot = 0;
        private const int AI_Timer_Slot = 1;
        private const int AI_Slot_2 = 2;
        private const int AI_Slot_3 = 3;

        // State values
        public enum State
        {
            Initial = 0, Waiting, Phase2Start, Crying
        }

        private const float speed = 14f;
        private const float acceleration = 1.2f;

        // Getters and Setters for AI slots for convenience
        public State AI_State
        {
            get => (State)npc.ai[AI_State_Slot];
            set => npc.ai[AI_State_Slot] = (float)value;
        }

        public float AI_Timer
        {
            get => npc.ai[AI_Timer_Slot];
            set => npc.ai[AI_Timer_Slot] = value;
        }

        public float AI_2
        {
            get => npc.ai[AI_Slot_2];
            set => npc.ai[AI_Slot_2] = value;
        }

        public float AI_3
        {
            get => npc.ai[AI_Slot_3];
            set => npc.ai[AI_Slot_3] = value;
        }

        private Player Target
        {
            get => Main.player[npc.target];
            set => npc.target = value.whoAmI;
        }

        public float Speed
        {
            get => npc.velocity.Length();
            set { npc.velocity.Normalize(); npc.velocity *= value; }
        }

        protected const int PrimaryEyeID = 0;
        protected OculusPrimaryEye PrimaryEye => (OculusPrimaryEye)Eyes[PrimaryEyeID];

        private void SpawnEyes()
        {
            int lastSlot = Main.npc.Length - 1;

            Eyes = new OculusEye[5];
            Eyes[PrimaryEyeID] = (OculusPrimaryEye)Main.npc[NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, NPCType<OculusPrimaryEye>(), 0, npc.whoAmI)].modNPC;
            for (int i = 1; i < 5; i++)
            {
                Eyes[i] = (OculusBasicEye)Main.npc[
                    NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, NPCType<OculusBasicEye>(), 0, npc.whoAmI, 0, 0, i)].modNPC;
            }

            eyesSpawned = true;
        }

        private void Cry()
        {
            if (AI_Timer > 900)
            {
                AI_Timer = Main.expertMode ? 120 : 240;
                AI_State = State.Waiting;
                return;
            }

            if (Target == null || !Target.active || Target.dead)
            {
                npc.TargetClosest(false);
            }

            Vector2 targetPosition = Target.Center;
            targetPosition.Y -= 160;
            int inertia = 20;
            Vector2 direction = targetPosition - npc.Center;
            direction.Normalize();
            direction *= speed;
            npc.velocity = (npc.velocity * (inertia - 1) + direction) / inertia;
        }

        private void Wait()
        {
            AI_Timer -= 2;
            npc.velocity *= .99f;
            if (AI_Timer <= 0)
            {
                AI_Timer = 0;
                AI_State = State.Crying;
            }
        }

        private void WaitPhase2()
        {
            AI_Timer -= 2;
            if (AI_Timer <= 0)
            {
                npc.rotation = 0;
                AI_Timer = 0;
            }
            else if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 away = npc.Center - Target.Center;
                away.Normalize();
                away *= 0.25f * speed;
                npc.velocity = away;
                npc.rotation *= npc.direction * npc.velocity.X * 0.05f;
            }
        }

        private void SecureSlot()
        {
            int highID = npc.whoAmI;

            foreach (OculusEye eye in Eyes)
            {
                if (eye.npc.whoAmI > highID)
                {
                    highID = eye.npc.whoAmI;
                }
            }
            //Swap Oculus into the highest Main.npc position available
            if (npc.whoAmI != highID)
            {
                Main.NewText("swapping");
                NPC other = Main.npc[highID];
                Main.npc[npc.whoAmI] = other;
                Main.npc[highID] = npc;

                // Swap the whoAmIs
                other.whoAmI = npc.whoAmI;
                npc.whoAmI = highID;
            }

            foreach (OculusEye eye in Eyes) eye.AI_Master = npc.whoAmI;

            slotSecured = true;
        }

        private bool eyesSpawned = false;
        private bool slotSecured = false;

        private void Initialize()
        {
            if (!eyesSpawned)
            {
                SpawnEyes();
            }
            else if (!slotSecured)
            {
                SecureSlot();
            }
            else
            {
                AI_State = State.Crying;
                AI_Timer = 0;
            }
        }

        public override void AI()
        {
            AI_Timer++;
            //Main.NewText(AI_State);

            if (hasEyes)
            {
                switch (AI_State)
                {
                    case State.Initial:
                        Initialize();
                        break;

                    case State.Waiting:
                        Wait();
                        break;

                    case State.Crying:
                        Cry();
                        break;
                }

                hasEyes = Eyes.Any(x => x.npc.active && x.attached);
                if (!hasEyes)
                {
                    AI_State = State.Phase2Start;
                    AI_Timer = 0;
                    npc.velocity *= 0;
                    Main.PlaySound(SoundID.ForceRoar, npc.Center);
                }
            }
            else
            {
                switch (AI_State)
                {
                    case State.Phase2Start:
                        if (AI_Timer < 720)
                        {
                            npc.rotation += MathHelper.Pi / 16;
                        }
                        else
                        {
                            AI_Timer = 120;
                            AI_State = State.Waiting;
                            AI_3 = float.MaxValue;
                        }
                        break;

                    case State.Waiting:
                        WaitPhase2();
                        break;

                    default:
                        Main.NewText("Error, invalid AI state in phase 2");
                        break;
                }
            }
        }
    }
}