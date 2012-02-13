using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    class Enemy
    {
        private Texture2D shotSprite;
        public bool start = false;
        public int appearTime = 0;
        public int xPos;
        public int yPos;
        public Texture2D sprite;
        public Rectangle hitBox;
        protected int spriteWidth;
        protected int spriteHeight;
        public bool alive = true;
        public int health;
        public double rotSpeed;
        public int rotAngle;
        public int aimTolerance;
        public double maxRotSpeed;
        public int moveSpeed;
        public ArrayList shotList = new ArrayList();

        public Enemy()
        {
        }
        /// <summary>
        /// Creates a new instance of an Enemy
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="spriteWidth"></param>
        /// <param name="spriteHeight"></param>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        public Enemy(Texture2D sprite, int spriteWidth, int spriteHeight, int xPos, int yPos, int health, int moveSpeed)
        {
            this.sprite = sprite;
            this.spriteHeight = spriteHeight;
            this.spriteWidth = spriteWidth;
            this.xPos = xPos;
            this.yPos = yPos;
            this.hitBox = new Rectangle(xPos, yPos, spriteWidth, spriteHeight);
            this.health = health;
            this.rotSpeed = 1;
            this.rotAngle = 180;
            this.aimTolerance = 0;
            this.maxRotSpeed = 0.5;
            this.moveSpeed = moveSpeed;
        }

        public void init(Texture2D sprite, Texture2D shotSprite, int spriteHeight, int spriteWidth)
        {
            this.sprite = sprite;
            this.shotSprite = shotSprite;
            this.spriteHeight = spriteHeight;
            this.spriteWidth = spriteWidth;
            this.hitBox = new Rectangle(xPos, yPos, spriteWidth, spriteHeight);
        }

        /// <summary>
        /// Updates the enemy's position, damage, and alive statues
        /// </summary>
        /// <param name="human"></param>
        public virtual void update(Player human)
        {
            foreach (Shot shot in human.shotList)
            {
                if (hitBox.Intersects(shot.hitBox))
                {
                    // subtract health from hit
                    health = health - shot.damage;

                    // if no health, set to dead
                    if (health <= 0)
                    {
                        alive = false;
                    }

                    // regester that the shot has connected
                    shot.hit = true;
                }
            }
        }
        /// <summary>
        /// Draws the enemy on to the screen
        /// </summary>
        /// <param name="batch"></param>
        public virtual void draw(SpriteBatch batch)
        {
            int drawx = xPos;
            int drawy = yPos;
            Rectangle drawrect = new Rectangle(drawx, drawy, spriteWidth, spriteHeight);
            batch.Draw(sprite, drawrect, Color.White);
        }

        public void drawShots(SpriteBatch batch)
        {
            foreach (EnemyShot shot in shotList)
            {
                shot.draw(batch);
            }
        }

        public void die(Expolsion explosion, SpriteBatch batch)
        {
            explosion.xPos = xPos;
            explosion.yPos = yPos;
            //explosion.draw(batch);
        }

        public virtual float getAngleToHuman(Player human)
        {
            float angle = 0;
            //get angle to human need to fix
            int a = Math.Abs(xPos + (spriteWidth / 2) - (human.xPos + human.sprite.spriteWidth / 2));
            int b = Math.Abs(yPos + (spriteHeight / 2) - (human.yPos + human.sprite.spriteHeight / 2));

            double c = Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
            angle = (float)Math.Sin(a / c);

            //TODO find way to make angle transitions more smooth

            //if human is up and left of enemy
            if (human.xPos < xPos && human.yPos < yPos)
            {
                angle = angle + 90;
            }
            //if human is down and left of enemy
            else if (human.xPos > xPos && human.yPos < yPos)
            {
                angle = angle + 180;
            }
            else if (human.xPos > xPos && human.yPos > yPos)
            {
                angle = angle + 270;
            }

            
            return angle;
        }

        /// <summary>
        /// Moves the enemy to the specified position on the screen at speed 1. Will change to have variable speed later
        /// </summary>
        /// <param name="tarX"></param>
        /// <param name="tarY"></param>
        /// <returns></returns>
        public virtual bool moveTo(int tarX, int tarY)
        {
            //set finished to false
            bool finished = false;
            if (xPos > tarX)
            {
                xPos = xPos - (1 * moveSpeed);
            }
            else if (xPos < tarX)
            {
                xPos = xPos + (1 * moveSpeed);
            }

            if (yPos < tarY)
            {
                yPos = yPos + (1 * moveSpeed);
            }
            else if (yPos > tarY)
            {
                yPos = yPos - (1 * moveSpeed);
            }
            //Otherwise, destiniation is reached, so finished is true
            else
            {
                finished = true;
            }

            //update hitbox
            hitBox.X = xPos;
            hitBox.Y = yPos;

            return finished;
        }

        /// <summary>
        /// Returns if the enemy is aimed within the tolerance 
        /// </summary>
        /// <param name="human"></param>
        /// <returns></returns>
        public virtual bool isAimedAt(Player human, float humanAngle)
        {
            //if (rotAngle <= humanAngle + aimTolerance || rotAngle >= humanAngle - aimTolerance)
            if (rotAngle == humanAngle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool aim(Player human)
        {
            float humanAngle = getAngleToHuman(human);
            
            //if not aimed at human
            if (!isAimedAt(human, humanAngle))
            {
                if (rotAngle <= humanAngle)
                {
                    this.rotSpeed = maxRotSpeed;
                    return false;
                }
                else
                {
                    this.rotSpeed = -1 * maxRotSpeed;
                    return false;
                }
            }
            else
            {
                this.rotSpeed = 0;
                return true;
            }
        }
    }
}
