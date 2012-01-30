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
        private Vector2 origin;
        public float rotAngle = 0;

        private delegate void Del();
        
        public Grunt()
        {
        }

        public Grunt(Texture2D sprite, int spriteWidth, int spriteHeight, int xPos, int yPos, int health)
        {
            this.alive = true;
            this.sprite = sprite;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            this.xPos = xPos;
            this.yPos = yPos;
            this.health = health;
            this.origin = new Vector2(spriteWidth / 2, spriteHeight / 2);
            this.hitBox = new Rectangle(xPos, yPos, spriteWidth, spriteHeight);
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

            rotAngle = getAngleToHuman(human);
        }

        public override void draw(SpriteBatch batch)
        {
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
