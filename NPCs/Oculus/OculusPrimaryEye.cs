namespace Desolation.NPCs.Oculus
{
    internal class OculusPrimaryEye : OculusEye
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            // Cosmetic
            npc.width = 36;
            npc.height = 36;

            // Combat
            npc.damage = 30;
            npc.defense = 30;
            npc.lifeMax = 12000;
        }

        public override void AI()
        {
            base.AI();
        }
    }
}