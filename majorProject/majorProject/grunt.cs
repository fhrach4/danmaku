﻿using System;
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
    class Grunt : Enemy
    {
        private Vector2 origin;
        public float rotAngle = 0;
        public double rotSpeed;
        private double maxRotSpeed;

        private short moveID = 0;

        public int appearTime;

        private delegate void Del();
        
        public Grunt()
        {
        }

        public Grunt(Texture2D sprite, int spriteWidth, int spriteHeight, int xPos, int yPos, int health, double maxRotSpeed)
        {
            this.alive = true;
            this.sprite = sprite;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.xPos = xPos;
            this.yPos = yPos;
            this.health = health;
            this.hitBox = new Rectangle(xPos - spriteWidth / 2, yPos - spriteHeight / 2, spriteWidth, spriteHeight);
            this.origin = new Vector2(spriteWidth / 2, spriteHeight / 2);
            this.rotSpeed = 0;
            this.maxRotSpeed = 0.1;
        }

        public override void update(Player human)
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

            // Aiming Code
            moveID = 1;

            //if (rotAngle >= 360 || rotAngle < 0)
            //{
            //    rotAngle = 0;
            //}
        
            if (moveID == 0)
            {
                if(moveTo(0,0))
                 {
                    moveID++;
                 }
            }else if(moveID == 1)
            {
                if (aim(human))
                {
                    moveID++;
                }
            }

            //handle rotation
            this.rotAngle = rotAngle + (float)rotSpeed;
            
        }

        public virtual bool aim(Player human)
        {
            float humanAngle = getAngleToHuman(human);

            //if not aimed at human
            if (!isAimedAt(human, humanAngle))
            {
                if (rotAngle <= humanAngle)
                {
                    rotSpeed = maxRotSpeed;
                    return false;
                }
                else
                {
                    rotSpeed = -1 * maxRotSpeed;
                    return false;
                }
            }
            else
            {
                rotSpeed = 0;
                return true;
            }
        }

        public override bool isAimedAt(Player human, float humanAngle)
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

        public override void draw(SpriteBatch batch)
        {
            // convert angle to radians
            float convert = MathHelper.Pi * 2;
            float finangle = rotAngle % convert;
            Vector2 pos = new Vector2(xPos, yPos);
            int drawx = xPos;
            int drawy = yPos;
            Rectangle drawrect = new Rectangle(drawx, drawy, spriteWidth, spriteHeight);
            batch.Draw(sprite, pos, null, Color.White, rotAngle, origin, 1.0f, SpriteEffects.None, 0f);
        }

        public void moveForward()
        {
            yPos++;
        }

        public void moveBackward()
        {
            yPos--;
        }


    }
}
