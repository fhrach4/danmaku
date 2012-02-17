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

namespace majorProject
{
    class Expolsion
    {
        //Private globals
        private AnimatedSprite sprite;
        public int xPos;
        public int yPos;

        public bool finished = false;

        /// <summary>
        /// Creates a new explosion
        /// </summary>
        /// <param name="sprite">animated sprite to use</param>
        /// <param name="spriteWidth">the frame width</param>
        /// <param name="spriteHeight">the frame height</param>
        /// <param name="frames">the number of frames</param>
        public Expolsion(Texture2D sprite, int spriteWidth, int spriteHeight, int frames)
        {
            this.sprite = new AnimatedSprite(sprite, 0, 0, frames, spriteWidth, spriteHeight);
        }

        /// <summary>
        /// Updates the explosion animation
        /// </summary>
        public void update()
        {
            if (sprite.currentFrame <= sprite.maxFrame)
            {
                sprite.currentFrame++;
            }
            else
            {
                finished = true;
            }
        }

        /// <summary>
        /// Draws the explosion in the world
        /// </summary>
        /// <param name="batch">current sprite batch</param>
        public void draw(SpriteBatch batch)
        {
            int drawX = sprite.currentFrame * sprite.spriteWidth;
            int drawY = 0;

            Rectangle spriteRect = new Rectangle(drawX, drawY, sprite.spriteWidth, sprite.spriteHeight);
            Rectangle drawRect = new Rectangle(xPos - 50, yPos - 50, sprite.spriteWidth, sprite.spriteHeight);

            batch.Draw(sprite.texture, drawRect, spriteRect, Color.White);
        }
    }
}
