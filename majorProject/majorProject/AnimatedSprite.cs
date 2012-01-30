/* File: AnimatedSprite.cs
 * Author: Frank Hrach (fjh3938@rit.edu)
 * Description: a general animated sprite class, but designed for danmaku
 */
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Component that handles 2D animation based on multiple frames 
    /// held within a single texture.
    /// 
    /// Both movement and Speed are tracked by the Animated Sprite class
    /// </summary>
    public class AnimatedSprite
    {
        //protected class variables
        
        public int maxFrame;

        //public class variables
        public Texture2D texture;
        public int spriteWidth;
        public int spriteHeight;
        public int currentFrame;
        public int neutralFrame;

        //keyboard states
        KeyboardState currentKBState;
        KeyboardState previousKBState;

        //constructor
        public AnimatedSprite(Texture2D texture, int currentFrame, int neutralFrame, int maxFrame, int spriteWidth, int spriteHeight)
        {
            this.texture = texture;
            this.currentFrame = currentFrame;
            this.neutralFrame = neutralFrame;
            this.maxFrame = maxFrame;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteWidth;

        }

        public void handleMovement(int speed)
        {
            //Update the keyboard states
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            Rectangle sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            
            //if no keys pressed, set to neutral frame
            if (currentKBState.GetPressedKeys().Length == 0)
            {
                currentFrame = neutralFrame;
            }
            //otherwise check frames
            else
            {
                //if inside frame bounds
                if (currentFrame >= 0 || currentFrame <= maxFrame)
                {
                    if (speed > 0 && currentFrame < maxFrame)
                    {
                        currentFrame++;
                    }
                    else if (speed < 0 && currentFrame > 0)
                    {
                        currentFrame--;
                    }
                }
            }
        }

        public void draw(SpriteBatch batch, Vector2 vect)
        {
            int drawx = currentFrame * spriteWidth;
            int drawy = 0;
            Rectangle spriterect = new Rectangle(drawx, drawy, spriteWidth, spriteHeight);
            Rectangle drawrect = new Rectangle((int)vect.X, (int)vect.Y, spriteWidth, spriteHeight);

            batch.Draw(texture, drawrect, spriterect, Color.White);
        }

        public void returnToNeutral()
        {
            if (currentFrame > neutralFrame)
            {
                currentFrame--;
            }
            else if (currentFrame < neutralFrame)
            {
                currentFrame++;
            }
        }

    }
}
