using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

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
    class Boss : Enemy
    {
        private List<Shot> shotTypes = new List<Shot>();

        private int action = 0;
        private int tarx;
        private int tary;

        public Boss(int xPos, int yPos, int health, int tarx, int tary)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.health = health;
            this.tarx = tarx;
            this.tary = tary;

        }

        public Boss(Texture2D sprite, List<Shot> shotTypes, int spriteWidth, int spriteHeight, int xPos, int yPos, int health,
            int tarx, int tary, Constants constants)
        {
            // Sprite variables
            this.sprite = sprite;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;
            
            // Shot variables
            this.shotTypes = shotTypes;

            // Other
            this.xPos = xPos;
            this.yPos = yPos;
            this.health = health;

            this.tarx = tarx;
            this.tary = tary;
        }

        public override void update(Player human, EnemyShot[] shotList)
        {
            bool complete = false;

            base.update(human, shotList);

            if (action == 0)
            {
                base.moveTo(300, 300);
            }
            else if (action == 1)
            {
                //Do other stuff
            }
            else if (action == 2)
            {
                //Do differant stuff
            }

            Console.WriteLine("Boss Health: " + Convert.ToString(health));

            if (complete)
            {
                action++;
            }
        }

        public override void die(Expolsion explosion, SpriteBatch batch)
        {
            base.die(explosion, batch);
            xPos = -100;
            yPos = -100;
        }
    }
}
