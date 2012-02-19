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
    class Grunt : Enemy
    {
        //private sprite vaibles
        private Vector2 origin;
        //public double rotSpeed;
        //private double maxRotSpeed;

        //private shot variables
        private int shotSpeed;

        //private movement and rotation variables
        private short moveID = 0;
        private new float rotAngle = 0;

        //public movememt variables
        public int tarx;
        public int tary;

        public ArrayList removeList = new ArrayList();
        
        public Grunt()
        {
        }

        public Grunt(Texture2D sprite, Texture2D shotSprite, int spriteWidth, int spriteHeight, int xPos, int yPos, int health, double maxRotSpeed, Constants constants)
        {
            this.alive = true;
            this.sprite = sprite;
            this.shotSprite = shotSprite;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.xPos = xPos;
            this.yPos = yPos;
            this.health = health;
            this.hitBox = new Rectangle(xPos - spriteWidth / 2, yPos - spriteHeight / 2, spriteWidth, spriteHeight);
            this.origin = new Vector2(spriteWidth / 2, spriteHeight / 2);
            this.rotSpeed = 0;
            this.maxRotSpeed = 0.1;
            this.shotSpeed = 3;
            this.constants = constants;
        }

        public override void update(Player human, EnemyShot[] shotList)
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
            //moveID = 1;

            //if (rotAngle >= 360 || rotAngle < 0)
            //{
            //    rotAngle = 0;
            //}
        
            if (moveID == 0)
            {
                if(moveTo(tarx, tary))
                 {
                    moveID++;
                    shoot(ref shotList);
                 }
            }else if(moveID == 1)
            {
                if (aim(human))
                {
                    //moveID++;
                }

                moveID++;

            }
            else if (moveID == 2)
            {
                if (moveTo(tarx, -110))
                {
                    moveID++;
                }
            }
            else
            {
                alive = false;
            }
            //handle rotation
            this.rotAngle = rotAngle + (float)rotSpeed;

            //update shots

            
        }

        public void updateShots()
        {
            foreach (EnemyShot shot in shotList)
            {
                if (shot.isOutOfPlay() || shot.hit)
                {
                    removeList.Add(shot);
                }
            }

            foreach (EnemyShot shot in removeList)
            {
                shotList.Remove(shot);
            }
        }

        public void shoot(ref EnemyShot[] shotlist)
        {
            //rotAngle = 2;
            EnemyShot shot = new EnemyShot(shotSprite, 4, (int)rotAngle, constants.ENEMY_SHOT_MAXSPEED  - 1,
                xPos, yPos, constants.ENEMY_SHOT_HEIGHT, constants.ENEMY_SHOT_HEIGHT);
            for (int i = 0; i < shotlist.Length; i++)
            {
                if (shotlist[i] == null)
                {
                    shotlist[i] = shot;
                    break;
                }
            }
        }



        public new bool aim(Player human)
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
            batch.Draw(sprite, pos, null, Color.White, rotAngle, origin, 1.0f, SpriteEffects.None, 0);
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
