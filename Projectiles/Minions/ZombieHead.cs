using Desolation.Buffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace Desolation.Projectiles.Minions
{
    internal class ZombieHead : ModProjectile
    {
        protected static float TargetRange = 800f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
        }

        public sealed override void SetDefaults()
        {
            // Size
            projectile.width = 24;
            projectile.height = 24;

            // Minion
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1f;
            projectile.penetrate = -1;

            projectile.tileCollide = false;

            projectile.frame = Main.rand.Next(0, 6);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        Player player => Main.player[projectile.owner];

        private Vector2 IdlePosition => new Vector2(
            player.Center.X + ((10 + projectile.minionPos * 40) * -player.direction),
            player.Center.Y - 48);

        private void MaintainDistance()
        {
            // Teleport to player if distance is too big
            Vector2 ToIdle = IdlePosition - projectile.Center;
            float distanceToIdlePosition = ToIdle.Length();
            if (Main.myPlayer == player.whoAmI && distanceToIdlePosition > 2000f)
            {
                projectile.position = IdlePosition;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }

            // Fix overlap with other minions
            float overlapVelocity = 0.04f;
            foreach (Projectile other in Main.projectile)
            {
                if (other != null && other.whoAmI != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
                {
                    if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
                    else projectile.velocity.X += overlapVelocity;

                    if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
                    else projectile.velocity.Y += overlapVelocity;
                }
            }
        }

        private Vector2 FindTarget()
        {
            // Starting search distance
            int target = -1;
            bool foundTarget = false;

            // See if the player gave a minion target
            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float dist = Vector2.Distance(npc.Center, projectile.Center);
                // Reasonable distance away so it doesn't target across multiple screens
                if (dist < 2000f)
                {
                    target = npc.whoAmI;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                foreach (NPC npc in Main.npc)
                {
                    if (npc != null && npc.CanBeChasedBy())
                    {
                        float dist = Vector2.Distance(npc.Center, projectile.Center);
                        bool closest = target == -1 || Vector2.Distance(projectile.Center, Main.npc[target].Center) > dist;
                        bool inRange = dist < TargetRange;
                        bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
                        bool closeThroughWall = dist < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            target = npc.whoAmI;
                            foundTarget = true;
                        }
                    }
                }
            }

            projectile.friendly = foundTarget;
            return target == -1 ? Vector2.Zero : Main.npc[target].Center;
        }

        private void Move()
        {
            // Default movement parameters (here for attacking)
            float speed = 8f;
            float inertia = 20f;
            Vector2 targetPosition = FindTarget();
            if (targetPosition != Vector2.Zero)
            {
                // Melee range, prevents latching
                if (Vector2.Distance(targetPosition, projectile.Center) > 40f)
                {
                    Vector2 direction = targetPosition - projectile.Center;
                    direction.Normalize();
                    direction *= speed;
                    projectile.velocity = (projectile.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            else
            {
                Vector2 idlePos = IdlePosition;
                float idleDist = Vector2.Distance(projectile.Center, idlePos);

                if (idleDist > 600f)
                { // Minion far, close distance quickly
                    speed = 12f;
                    inertia = 60f;
                }
                else
                { // Minion close, slow down
                    speed = 4f;
                    inertia = 80f;
                }
                if (idleDist > 20f)
                { // Very close but not exactly there, float around
                    Vector2 ToIdle = idlePos - projectile.Center;
                    ToIdle.Normalize();
                    ToIdle *= speed;
                    projectile.velocity = (projectile.velocity * (inertia - 1) + ToIdle) / inertia;
                }
                else if (projectile.velocity == Vector2.Zero)
                { // Guarantee some slight movement
                    projectile.velocity.X = -0.15f;
                    projectile.velocity.Y = -0.05f;
                }
            }
        }

        private void Visuals()
        {
            projectile.spriteDirection = projectile.direction;
            // Lean slightly towards the direction it's moving
            projectile.rotation = projectile.velocity.X * 0.15f;
        }

        public override void AI()
        {
            #region Active check

            if (player.dead || !player.active)
            {
                player.ClearBuff(BuffType<ZombieHeadBuff>());
            }
            if (player.HasBuff(BuffType<ZombieHeadBuff>()))
            {
                projectile.timeLeft = 2;
            }

            #endregion Active check

            MaintainDistance();
            Move();
            Visuals();
        }
    }
}