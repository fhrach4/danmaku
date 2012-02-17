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
    /// <summary>
    /// Base class for shots
    /// </summary>
    class Shot
    {
        //Procteced Values
        protected int maxSpeed;
        protected Texture2D sprite;
        protected int spriteWidth;
        protected int spriteHeight;

        //Public values
        public int damage;
        public int xPos;
        public int yPos;
        public bool hit = false;
        public Rectangle hitBox;

        /// <summary>
        /// Blank constructor
        /// </summary>
        public Shot()
        {
        }

        /// <summary>
        /// Creates a new shot
        /// </summary>
        /// <param name="sprite">The sprite (non animated) to use for the shot</param>
        /// <param name="xPos">The starting x position of the sprite</param>
        /// <param name="yPos">The starting y position of the sprite</param>
        /// <param name="damage">The damage the shot will cause on impact</param>
        /// <param name="maxSpeed">The maximum travel speed of the shot</param>
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

        /// <summary>
        /// Updates the shot
        /// </summary>
        public virtual void update()
        {
            yPos = yPos - maxSpeed;
            hitBox.Y = yPos;
        }

        /// <summary>
        /// Checks to see if the shot is out of play
        /// </summary>
        /// <returns>True if shot is out of play, otherwise false</returns>
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

        /// <summary>
        /// Draws the shot
        /// </summary>
        /// <param name="batch">Current Sprite Batch</param>
        public virtual void draw(SpriteBatch batch)
        {
            int drawx = xPos;
            int drawy = yPos;
            Rectangle drawrect = new Rectangle(drawx, drawy, spriteWidth, spriteHeight);
            batch.Draw(sprite, drawrect, Color.White);
        }
    }
}
