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
    /// <summary>
    /// A human controlled player
    /// </summary>
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

        //sfx
        protected SoundEffect playerDie;

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

        // control interfaces
        public enum input
        {
            up      = Keys.Up,
            down    = Keys.Down,
            left    = Keys.Left,
            right   = Keys.Right,
            focus   = Keys.LeftShift,
            fire    = Keys.Z
        };

        /// <summary>
        /// Creates a new human controlled player
        /// </summary>
        /// <param name="sprite">The animatedSprite to be used</param>
        /// <param name="shotSprite">The sprite to be used for shots</param>
        /// <param name="xPos">The starting x position of the player</param>
        /// <param name="yPos">The starting y position of the player</param>
        /// <param name="maxSpeed">The maximum moving speed of the player</param>
        public Player(AnimatedSprite sprite,Texture2D shotSprite, int xPos, int yPos, int maxSpeed, SoundEffect die)
        {
            //set location variables
            this.xPos = xPos;
            this.yPos = yPos;
            this.spawnX = xPos;
            this.spawnY = yPos;
            
            // handle shots
            this.sprite = sprite;
            this.shotSprite = shotSprite;

            // handle speed
            this.currentXSpeed = 0;
            this.currentYSpeed = 0;
            this.maxSpeed = maxSpeed;

            // create hitbox
            hitBoxOffset = this.xPos + 10;
            this.hitBox = new Rectangle(hitBoxOffset, yPos, sprite.spriteWidth - 10, sprite.spriteHeight - 10);

            // set lives
            this.lives = 3;

            this.playerDie = die;

            // start timer
            shotTimer.Start();

            //eventually change so player  can define keys
        }

        /// <summary>
        /// Updates the player's state in the world
        /// </summary>
        /// <param name="time">The current gametime</param>
        /// <param name="enemyList">The current list onf enemies</param>
        /// <returns>new position of the player</returns>
        public Vector2 updateState(GameTime time, Enemy[] enemyList, Boss boss)
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
                        enemy.alive = false;
                    }
                }

                if (boss != null && hitBox.Intersects(boss.hitBox))
                {
                    boss.health = boss.health - 50;
                    hit = true;
                }
            }

            if (hit)
            {
                update = new Vector2(xPos,yPos);
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
            // if shooting and shot timer is ok
            if (KBstate.IsKeyDown((Keys)input.fire) && shotTimer.ElapsedMilliseconds >= shotDelay)
            {
                // #Shot 1
                // Chnage shot origin based on rotation
                // TODO: fix this, it's still broken
                int shot1x = xPos + Math.Abs(sprite.currentFrame - 6);
                int shot1y = yPos + 20;
                Shot shot = new Shot(shotSprite, shot1x, shot1y, 10, 15);

                // #Shot 2
                int shot2x = xPos + Math.Abs(sprite.currentFrame - 6) + 25;
                int shot2y = yPos + 20;
                Shot shot2 = new Shot(shotSprite, shot2x, shot2y, 10, 15);
                
                // add shots to the shot list
                shotList.Add(shot);
                shotList.Add(shot2);

                // reset the shot timer
                shotTimer.Restart();
            }
        }

        /// <summary>
        /// Handles keyboard input from the player
        /// </summary>
        /// <param name="KBstate">The current Keyboard state</param>
        public void keyboardMovement(KeyboardState KBstate)
        {
            //if up key is hit
                if (KBstate.IsKeyDown((Keys)input.up))
                {
                    if (yPos < 0)
                    {
                        currentYSpeed = 0;
                    }
                    else
                    {
                        currentYSpeed = maxSpeed * -1;
                    }
                }

                //if down key is hit
                if (KBstate.IsKeyDown((Keys)input.down))
                {
                    if (yPos > 600 - sprite.spriteHeight)
                    {
                        currentYSpeed = 0;
                    }
                    else
                    {
                        currentYSpeed = maxSpeed;
                    }
                }

                // if neither up nor down are hit
                if (! (KBstate.IsKeyDown((Keys)input.down) || KBstate.IsKeyDown((Keys)input.up)))
                {
                    currentYSpeed = 0;
                }

                // if left key is hit
                if (KBstate.IsKeyDown((Keys)input.left))
                {
                    if (xPos < 0)
                    {
                        currentXSpeed = 0;
                    }
                    else
                    {
                        currentXSpeed = maxSpeed * -1;
                    }
                }

                // if right key is hit
                if (KBstate.IsKeyDown((Keys)input.right))
                {
                    if (xPos > 800 - sprite.spriteWidth)
                    {
                        currentXSpeed = 0;
                    }
                    else
                    {
                        currentXSpeed = maxSpeed;
                    }
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

                // change speed based off focus state and move
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

        /// <summary>
        /// Draws the player's shots
        /// </summary>
        /// <param name="batch">Current sprite batch</param>
        public void drawShots(SpriteBatch batch)
        {
            foreach (Shot shot in shotList)
            {
                shot.draw(batch);
            }
        }

        /// <summary>
        /// Creates an explosion at the player's position
        /// </summary>
        /// <param name="explosion"></param>
        /// <param name="batch"></param>
        public void die(Expolsion explosion, SpriteBatch batch)
        {
            explosion.xPos = xPos;
            explosion.yPos = yPos;
            playerDie.Play();
        }
        
        /// <summary>
        /// Handles player respawns
        /// </summary>
        public void respawnUpdate()
        {
            // If the player has lives left
            if (lives >= 0)
            {
                // if hit, return to spawn position
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

                // If respwan period is over, set the player back to normal
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

        public int getQuadrant()
        {
            if (xPos < 400)
            {
                if (yPos < 300)
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                if (yPos < 300)
                {
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
        }
    }
}
