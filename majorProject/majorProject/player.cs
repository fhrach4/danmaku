/* File: player.cs
 * Author: Frank Hrach (fjh3938@rit.edu)
 * Description: the human player class for damnaku
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

//XNA imports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace majorProject
{
    class Player
    {
        //## protected values
        public ArrayList shotList = new ArrayList();
        public ArrayList removeList = new ArrayList();
        protected int maxSpeed;
        protected int shotDelay = 100;
        protected double timeSinceLastShot = 0;
        protected Stopwatch shotTimer = new Stopwatch();

        //public sprite values
        public AnimatedSprite sprite;
        public Texture2D shotSprite;
        
        //position variables
        public int xPos;
        public int yPos;

        //speed variables
        public int currentXSpeed;
        public int currentYSpeed;

        //collision variables
        public Rectangle hitBox;
        private int hitBoxOffset;
        public bool hit = false;
        protected Stopwatch respawnTimer = new Stopwatch();

        //Spawn point
        private int spawnX;
        private int spawnY;

        //Other
        public int lives;
        public int score;
        public bool respawn = false;

        public enum input
        {
            up      = Keys.Up,
            down    = Keys.Down,
            left    = Keys.Left,
            right   = Keys.Right,
            focus   = Keys.LeftShift,
            fire    = Keys.Z
        };

        public Player(AnimatedSprite sprite,Texture2D shotSprite, int xPos, int yPos, int maxSpeed)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.spawnX = xPos;
            this.spawnY = yPos;
            this.sprite = sprite;
            this.shotSprite = shotSprite;
            this.currentXSpeed = 0;
            this.currentYSpeed = 0;
            this.maxSpeed = maxSpeed;
            hitBoxOffset = this.xPos + 10;
            this.hitBox = new Rectangle(hitBoxOffset, yPos, sprite.spriteWidth - 10, sprite.spriteHeight - 10);
            this.lives = 3;

            shotTimer.Start();

            //eventually change so player  can define keys
        }

        public Vector2 updateState(GameTime time, Enemy[] enemyList)
        {
            Vector2 update;
            //get a list of pressed keys
            KeyboardState KBstate = Keyboard.GetState();

            //if not respawning, handle collisions
            if (!respawn)
            {
                foreach (Enemy enemy in enemyList)
                {
                    if (enemy != null && hitBox.Intersects(enemy.hitBox))
                    {
                        hit = true;
                    }
                }
            }

            if (hit)
            {
                update = new Vector2(xPos, yPos);
                respawn = true;
                respawnTimer.Start();
                lives--;
            }
            else if (respawn)
            {
                keyboardMovement(KBstate);
            }
            else
            {

                keyboardMovement(KBstate);
                keyboardShoot(KBstate);
            }
            //update each shot, and clear it if it is out of bounds
            
            foreach(Shot shot in shotList)
            {
                shot.update();
                if (shot.isOutOfPlay() || shot.hit)
                {
                    removeList.Add(shot);
                }
            }

            foreach (Shot shot in removeList)
            {
                shotList.Remove(shot);
            }
            


            //update hitbox
            hitBox.X = xPos + 5;
            hitBox.Y = yPos + 10;

            update = new Vector2(xPos, yPos);

            //handle sprite animation
            sprite.handleMovement(currentXSpeed);
        return update;
        }

        public void keyboardShoot(KeyboardState KBstate)
        {
            // if shooting
            if (KBstate.IsKeyDown((Keys)input.fire) && shotTimer.ElapsedMilliseconds >= shotDelay)
            {
                int shot1x = xPos + Math.Abs(sprite.currentFrame - 6);
                int shot1y = yPos + 20;

                int shot2x = xPos + Math.Abs(sprite.currentFrame - 6) + 25;
                int shot2y = yPos + 20;
                Shot shot = new Shot(shotSprite, shot1x, shot1y, 10, 15);
                Shot shot2 = new Shot(shotSprite, shot2x, shot2y, 10, 15);
                shotList.Add(shot);
                shotList.Add(shot2);

                shotTimer.Restart();
            }
        }

        public void keyboardMovement(KeyboardState KBstate)
        {
            //if up key is hit
                if (KBstate.IsKeyDown((Keys)input.up) && currentYSpeed >= maxSpeed * -1)
                {
                    currentYSpeed--;
                }

                //if down key is hit
                if (KBstate.IsKeyDown((Keys)input.down) && currentYSpeed <= maxSpeed)
                {
                    currentYSpeed++;
                }

                // if neither up nor down are hit
                if (! (KBstate.IsKeyDown((Keys)input.down) || KBstate.IsKeyDown((Keys)input.up)))
                {
                    currentYSpeed = 0;
                }

                // if left key is hit
                if (KBstate.IsKeyDown((Keys)input.left) && currentXSpeed >= maxSpeed * -1)
                {

                    currentXSpeed--;
                }

                // if right key is hit
                if (KBstate.IsKeyDown((Keys)input.right) && currentXSpeed <= maxSpeed)
                {
                    currentXSpeed++;
                }

                // if neither left nor right are hit
                if (!(KBstate.IsKeyDown((Keys)input.left) || KBstate.IsKeyDown((Keys)input.right)))
                {
                    currentXSpeed = 0;
                    if(!(KBstate.IsKeyDown((Keys)input.focus)))
                    {
                        sprite.returnToNeutral();
                    }
                }

                // change speed based off focus state
                if (KBstate.IsKeyDown((Keys)input.focus))
                {
                    xPos = xPos + currentXSpeed / 3;
                    yPos = yPos + currentYSpeed / 3;
                }
                else
                {
                    xPos = xPos + currentXSpeed;
                    yPos = yPos + currentYSpeed;
                }
        }

        public void drawShots(SpriteBatch batch)
        {
            foreach (Shot shot in shotList)
            {
                shot.draw(batch);
            }
        }

        public void die(Expolsion explosion, SpriteBatch batch)
        {
            explosion.xPos = xPos;
            explosion.yPos = yPos;
        }
        
        public void respawnUpdate()
        {
            if (lives >= 0)
            {
                if (hit)
                {
                    if (xPos != spawnX)
                    {
                        xPos = spawnX;
                    }

                    if (yPos != spawnY)
                    {
                        yPos = spawnY;
                    }

                    hit = false;
                }


                if (respawnTimer.ElapsedMilliseconds >= 3000)
                {
                    respawn = false;
                    respawnTimer.Stop();
                    respawnTimer.Reset();
                }
            }
            else
            {
                //TODO:GAME OVER CODE
            }
        }
    }
}
