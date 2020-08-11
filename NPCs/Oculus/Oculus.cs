using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.NPCs.Oculus
{
    [AutoloadBossHead]
    internal class Oculus : ModNPC
    {
        private int myFrame = 0;

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
            Initial = 0, Waiting, Crying
        }

        private const float speed = 8f;
        private const float acceleration = 1.2f;

        // Getters and Setters for AI slots for convenience
        public float AI_State
        {
            get => npc.ai[AI_State_Slot];
            set => npc.ai[AI_State_Slot] = value;
        }

        public float AI_Timer
        {
            get => npc.ai[AI_Timer_Slot];
            set => npc.ai[AI_Timer_Slot] = value;
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

        private int PrimaryEyeID = -1;

        private void SpawnEyes()
        {
            PrimaryEyeID = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, NPCType<OculusPrimaryEye>(), 0, npc.whoAmI);
            if (PrimaryEyeID > npc.whoAmI) // Ensure the main body has the higher ID
            {
                int tmp = npc.whoAmI;
                NPC tmpNPC = Main.npc[PrimaryEyeID];
                npc.whoAmI = PrimaryEyeID;
                Main.npc[npc.whoAmI] = npc;
                tmpNPC.whoAmI = tmp;
                Main.npc[tmp] = tmpNPC;
                PrimaryEyeID = tmp;
                Main.npc[tmp].ai[0] = npc.whoAmI;
            }
        }

        public override void AI()
        {
            if (AI_State == (int)State.Initial)
            {
                SpawnEyes();
                AI_State = (int)State.Crying;
            }

            AI_Timer++;
        }
    }
}