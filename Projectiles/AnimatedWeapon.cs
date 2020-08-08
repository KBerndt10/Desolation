/*
 WIP
 */

using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace Desolation.Projectiles
{
    public class AnimatedWeapon : ModProjectile
    {
        protected int Proj = 0;

        public override void SetDefaults()
        {
            projectile.width = 17;
            projectile.height = 33;
            projectile.scale = 1f;
            projectile.aiStyle = 0;
            projectile.timeLeft = 999999;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            Main.projFrames[projectile.type] = 7;
        }

        private Player MyPlayer => Main.player[projectile.owner];
        private int Frames => Main.projFrames[projectile.type];

        public override void AI()
        {
            Main.NewText($"My player: {MyPlayer} owner: {projectile.owner}");
            // Updates position for owner only
            Vector2 angle = MyPlayer.RotatedRelativePoint(MyPlayer.MountedCenter, true);
            if (Main.myPlayer == projectile.owner)
            {
                if (MyPlayer.channel)
                {
                    float shootSpeed = MyPlayer.inventory[MyPlayer.selectedItem].shootSpeed * projectile.scale;
                    Vector2 origAngle = angle;
                    Vector2 toCursor = new Vector2
                    {
                        X = Main.mouseX + Main.screenPosition.X - origAngle.X,
                        Y = Main.mouseY + Main.screenPosition.Y - origAngle.Y
                    };
                    if (MyPlayer.gravDir == -1f)
                    {
                        // If player is upside down
                        toCursor.Y = Main.screenHeight - Main.mouseY + Main.screenPosition.Y - origAngle.Y;
                    }

                    float magnitude = (float)Math.Sqrt(toCursor.X * toCursor.X + toCursor.Y * toCursor.Y);
                    magnitude = (float)Math.Sqrt(toCursor.X * toCursor.X + toCursor.Y * toCursor.Y);
                    magnitude = shootSpeed / magnitude;
                    toCursor.X *= magnitude;
                    toCursor.Y *= magnitude;

                    // Flags the projectile for update if needed
                    if (toCursor.X != projectile.velocity.X || toCursor.Y != projectile.velocity.Y)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.velocity.X = toCursor.X;
                    projectile.velocity.Y = toCursor.Y;
                }
                else
                {
                    projectile.Kill();
                }
            }

            //Setting the position and direction
            if (projectile.velocity.X > 0f)
            {
                MyPlayer.ChangeDir(1);
            }
            else
            {
                if (projectile.velocity.X < 0f)
                {
                    MyPlayer.ChangeDir(-1);
                }
            }
            projectile.spriteDirection = projectile.direction;
            MyPlayer.ChangeDir(projectile.direction);
            MyPlayer.heldProj = projectile.whoAmI;
            projectile.position.X = angle.X - projectile.width / 2;
            projectile.position.Y = angle.Y - projectile.height / 2;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 2.355f;

            //Rotation of Projectile and Item Use
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= 1.57f;
            }
            if (MyPlayer.direction == 1)
            {
                MyPlayer.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
            }
            else
            {
                MyPlayer.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
            }

            //Make velocity really low towards mouse
            projectile.velocity.X = projectile.velocity.X * (1f + Main.rand.Next(-3, 4) * 0.01f);

            //Animation and firing in terms of frameCounter and first counter
            if (projectile.frameCounter % (int)projectile.ai[0] == 0)
            {
                //Animation
                if (projectile.frame < Frames - 1)
                {
                    projectile.frame++;
                }
                else
                {
                    projectile.frame = 0;
                }
            }

            projectile.frameCounter++;
            projectile.ai[1] += 1f;

            //Reset how long the projectile lives
            MyPlayer.itemTime = 10;
            MyPlayer.itemAnimation = 10;
            projectile.timeLeft = 999999;
        }
    }
}