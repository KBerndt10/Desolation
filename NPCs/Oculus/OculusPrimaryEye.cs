using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.NPCs.Oculus
{
    internal class OculusPrimaryEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 14;
        }

        public override void SetDefaults()
        {
            npc.value = 5000f;

            // Cosmetic
            npc.width = 36;
            npc.height = 36;
            animationType = -1;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.alpha = 0;

            // AI
            npc.aiStyle = -1;
            aiType = -1;
            npc.boss = true;

            // Combat
            npc.damage = 30;
            npc.defense = 30;
            npc.lifeMax = 12000;
            npc.knockBackResist = 0f;

            npc.noGravity = true;
        }

        // Which state in the npc.ai[] is which

        private const int AI_Master_Slot = 0;
        private const int AI_State_Slot = 1;
        private const int AI_Timer_Slot = 2;
        private const int AI_Slot_3 = 3;

        // State values
        private enum State
        {
            Waiting,
        }

        private const float speed = 8f;
        private const float acceleration = 1.2f;

        // Getters and Setters for AI slots for convenience
        public int AI_Master
        {
            get => (int)npc.ai[AI_Master_Slot];
            set => npc.ai[AI_Master_Slot] = value;
        }

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

        private NPC master => Main.npc[AI_Master];

        public override void AI()
        {
            // Get rid of this is the master is gone
            if (!master.active || master.type != NPCType<Oculus>())
            {
                npc.active = false;
            }

            // If over half life, stick to master
            if (npc.life > npc.lifeMax / 2)
            {
                npc.position = master.Center;
                npc.TargetClosest();
            }
        }
    }
}