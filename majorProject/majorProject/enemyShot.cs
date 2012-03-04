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
    /// Object similar to a normal shot, but for use only by enemies. Uses a circular hitbox
    /// </summary>
    class EnemyShot : Shot
    {
        // public variables
        public int xSpeed;
        public int ySpeed;
        public int angle;
        public int radius;

        // protected variables
        protected Vector2 origin;

        /// <summary>
        /// Creates a new enemy shot object
        /// </summary>
        /// <param name="sprite">The non-animated sprite to be used</param>
        /// <param name="radius">The radius of the sprite's hitbox</param>
        /// <param name="angle">The firing angle where 90 is vertical</param>
        /// <param name="maxSpeed">the maximum traveling speed of the shot</param>
        /// <param name="xPos">The starting x position</param>
        /// <param name="yPos">The starting y position</param>
        public EnemyShot(Texture2D sprite, int radius, int angle, int maxSpeed, int xPos, int yPos, int spriteHeight, int spriteWidth)
        {
            this.sprite = sprite;
            this.radius = radius;
            this.angle = angle;
            ySpeed = (int)(maxSpeed * Math.Cos(angle));
            xSpeed = (int)(maxSpeed * Math.Sin(angle));
            //ySpeed = (int)(maxSpeed * angle);
            //xSpeed = (int)(maxSpeed * angle);
            //ySpeed = 2;
            //xSpeed = 0;
            this.xPos = xPos;
            this.yPos = yPos;
            this.spriteHeight = spriteHeight;
            this.spriteWidth = spriteWidth;
            this.origin = new Vector2(spriteWidth / 2, spriteHeight / 2);
        }

        /// <summary>
        /// Updates the shot
        /// </summary>
         public override void update()
        {
            xPos = xPos + xSpeed;
            yPos = yPos + ySpeed;
        }

        /// <summary>
        /// Checks to see if there is a collision between the bullet and the player
        /// </summary>
        /// <param name="player">the player to check the collision against</param>
        /// <returns>True if there is a collision, otherwise false</returns>
         public bool collidesWith(Player player)
         {
             //get the distance to the sides of the player's hitbox
             int xDif1 = player.hitBox.X - (xPos + (int)origin.X);
             int xDif2 = (player.hitBox.X + player.hitBox.Width) - (xPos + (int)origin.X);

             int yDif1 = player.hitBox.Y - (yPos + (int)origin.Y);
             int yDif2 = (player.hitBox.Y + player.hitBox.Width) - (yPos + (int)origin.Y);
             
             // choose the smallest distance for x and y
             int xDif;
             int yDif;

             if (Math.Abs(xDif1) > Math.Abs(xDif2))
             {
                 xDif = xDif2;
             }else
             {
                 xDif = xDif1;
             }

             if (Math.Abs(yDif1) > Math.Abs(yDif2))
             {
                 yDif = yDif2;
             }
             else
             {
                 yDif = yDif1;
             }
             //int xDif = player.xPos - (xPos + (int)origin.X);
             //int yDif = player.yPos - (yPos + (int)origin.Y);

             if (-20 < xDif && xDif < 20 && -5 < yDif && yDif < 5)
             {
                 int i = 0;
             }

             double line = Math.Pow(xDif, 2) + Math.Pow(yDif, 2);

             if (line < radius * radius * radius)
             {
                 return true;
             }
             else
             {
                 return false;
             }
         }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="batch"></param>
         public override void draw(SpriteBatch batch)
         {
             int drawx = xPos;
             int drawy = yPos;
             Rectangle drawrect = new Rectangle(drawx, drawy, spriteWidth, spriteHeight);
             batch.Draw(sprite, drawrect, Color.White);
         }

    }
}
