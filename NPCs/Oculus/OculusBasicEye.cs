using Microsoft.Xna.Framework;

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
            attached = false;
        }

        public override void StickToMaster(Vector2 center)
        {
            npc.position = center;
            switch (AI_3)
            {
                case 1:
                    npc.position.X -= 43;
                    npc.position.Y += 27;

                    break;

                case 2:
                    npc.position.X += 67;
                    npc.position.Y += 39;
                    break;

                case 3:
                    npc.position.X -= Master.width * 0.4f; ;
                    npc.position.Y -= 8;
                    break;

                case 4:
                    npc.position.X += 25;
                    npc.position.Y -= 44;
                    break;

                default:
                    npc.position.X += 3;
                    npc.position.Y -= Master.height / 5;

                    break;
            }
            npc.netUpdate = true;
        }

        public override void AI()
        {
            base.AI();
        }
    }
}