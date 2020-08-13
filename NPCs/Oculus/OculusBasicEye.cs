namespace Desolation.NPCs.Oculus
{
    internal class OculusBasicEye : OculusEye
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            // Cosmetic
            npc.width = 16;
            npc.height = 16;

            // Combat
            npc.damage = 12;
            npc.defense = 20;
            npc.lifeMax = 1000;
        }

        public override void Detach()
        {
            npc.aiStyle = 2;
            npc.noTileCollide = false;
        }

        public override void StickToMaster()
        {
            npc.position = Master.Center;
            switch (AI_3)
            {
                case 1:
                    npc.position.X -= 14;
                    npc.position.Y += 22;

                    break;

                case 2:
                    npc.position.X += 17;
                    npc.position.Y += 19;
                    break;

                case 3:
                    npc.position.X -= Master.width * 0.9f; ;
                    npc.position.Y -= 2;
                    break;

                case 4:
                    npc.position.X += 5;
                    npc.position.Y -= 23;
                    break;

                default:
                    npc.position.X += 3;
                    npc.position.Y -= Master.height / 5;

                    break;
            }
        }

        public override void AI()
        {
            base.AI();
        }
    }
}