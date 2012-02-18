using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace majorProject
{
    class Constants
    {   
        //#PLAYER CONSTANTS
        //Player Movement Constants
        public readonly int MAX_HUMAN_SPEED = 7;
        public readonly int HUMAN_START_X = 300;
        public readonly int HUMAN_START_Y = 500;

        //Player Sprite Constants
        public readonly int MIN_HUMAN_FRAMES = 0;
        public readonly int MAX_HUMAN_FRAMES = 10;
        public readonly int HUMAN_NEUTRAL_FRAME = 5;
        public readonly int HUMAN_SPRITE_WIDTH = 34;
        public readonly int HUMAN_SPRITE_HEIGHT = 38;

        //#GRUNT CONSTANTS
        //Grunt sprite constants
        public readonly int GRUNT_SPRITE_WIDTH = 34;
        public readonly int GRUNT_SPRITE_HEIGHT = 38;

        //# shot constants
        //enemy shot
        public readonly int ENEMY_SHOT_HEIGHT = 30;
        public readonly int ENEMY_SHOT_WIDTH = 30;
        public readonly int ENEMY_SHOT_MAXSPEED = 2;

        public Constants()
        {
        }
    }
}
