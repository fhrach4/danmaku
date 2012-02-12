using System;
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
    class Shot
    {
        //Procteced Values
        protected int maxSpeed;
        public Texture2D sprite;
        protected int spriteWidth;
        protected int spriteHeight;

        //Public values
        public int damage;
        public int xPos;
        public int yPos;
        public bool hit = false;
        public Rectangle hitBox;

        public Shot()
        {
        }

        public Shot(Texture2D sprite, int xPos, int yPos, int damage, int maxSpeed)
        {
            this.sprite = sprite;
            this.xPos = xPos;
            this.yPos = yPos;
            this.maxSpeed = maxSpeed;
            this.spriteHeight = 8;
            this.spriteWidth = 4;
            this.damage = damage;
            this.hitBox = new Rectangle(xPos, yPos, spriteWidth, spriteHeight);
        }

        public virtual void update()
        {
            yPos = yPos - maxSpeed;
            hitBox.Y = yPos;
        }

        public bool isOutOfPlay()
        {
            if (yPos < 0 || yPos > 600 || xPos < 0 || xPos > 800)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void draw(SpriteBatch batch)
        {
            int drawx = xPos;
            int drawy = yPos;
            Rectangle drawrect = new Rectangle(drawx, drawy, spriteWidth, spriteHeight);
            batch.Draw(sprite, drawrect, Color.White);
        }
    }
}
