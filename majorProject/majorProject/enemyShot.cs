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
        public EnemyShot(Texture2D sprite, int radius, int angle, int maxSpeed, int xPos, int yPos)
        {
            this.sprite = sprite;
            this.radius = radius;
            this.angle = angle;
            xSpeed = (int)(maxSpeed * Math.Sin(maxSpeed));
            xSpeed = (int)(maxSpeed * Math.Cos(maxSpeed));
            this.xPos = xPos;
            this.yPos = yPos;
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
         public bool collidsWith(Player player)
         {
             int xDif = player.xPos - xPos;
             int yDif = player.yPos - yPos;

             double line = Math.Pow(xDif, 2) + Math.Pow(yDif, 2);

             if (line > radius * radius)
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
