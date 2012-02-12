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
    class EnemyShot : Shot
    {
        protected Texture2D sprite;
        public int xSpeed;
        public int ySpeed;
        public int angle;

        public int radius;
        private Vector2 origin;

        public EnemyShot(Texture2D sprite, int radius, int angle, int maxSpeed)
        {
            this.sprite = sprite;
            this.radius = radius;
            this.angle = angle;
            this.maxSpeed = maxSpeed;
        }

         public override void update()
        {
            xPos = xPos + xSpeed;
            yPos = yPos + ySpeed;
        }


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
    }
}
