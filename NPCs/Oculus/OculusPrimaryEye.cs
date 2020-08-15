using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

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

        public override void FindFrame(int frameHeight)
        {
            if (attached)
            {
                base.FindFrame(frameHeight);
                return;
            }
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
            if (LookAtPlayer && AI_State != State.Detaching)
            {
                npc.rotation = npc.AngleTo(Target.Center) + MathHelper.PiOver4;
            }
        }

        public override void Detach()
        {
            AI_Timer = 0;
            AI_State = State.Detaching;
            attached = false;
        }

        public override void StickToMaster(Vector2 center)
        {
            npc.position = center;
            npc.position.X -= npc.width;
            npc.netUpdate = true;
        }

        public void Pursue()
        {
            if (AI_Timer >= 400)
            {
                AI_Timer = 130;
                AI_State = State.Waiting;
                return;
            }

            npc.TargetClosest();

            Vector2 targetPosition = Target.Center;
            int inertia = 20;
            Vector2 direction = targetPosition - npc.Center;
            direction.Normalize();
            direction *= speed;
            npc.velocity += (direction * 0.03f);
            if (Speed > speed)
            {
                Speed = speed;
            }
        }

        public void BulletSprayLaser()
        {
            if (MasterTimer % 40 == 0)
            {
                npc.TargetClosest();
                Vector2 projVel = Target.Center - npc.Center;
                Projectile.NewProjectile(npc.Center, projVel, ProjectileID.EyeLaser, npc.damage, 3);
            }
        }

        public override void AI()
        {
            if (attached)
            {
                switch (MasterState)
                {
                    case Oculus.State.BulletSpray:
                        BulletSprayLaser();
                        break;

                    default:
                        base.AI();
                        break;
                }
            }
            else
            {
                AI_Timer++;
                switch (AI_State)
                {
                    case State.Detaching:
                        if (AI_Timer > 120)
                        {
                            AI_State = State.Pursue;
                            AI_Timer = 0;
                            npc.rotation = 0;
                        }
                        else
                        {
                            npc.rotation += MathHelper.Pi / 8;
                        }
                        break;

                    case State.Pursue:
                        Pursue();
                        break;

                    case State.Waiting:
                        Wait();
                        break;

                    default: break;
                }
            }
        }
    }
}