using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Regex ftw
using System.Text.RegularExpressions;
using System.IO;

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
    /// Object that reads .lvl files
    /// </summary>
    class LevelReader
    {
        private ArrayList levelFiles = new ArrayList();

        //public globals
        public string background;               //background stored as string to load in Load in Game1.cs
        public string levelSong;                //song stored as string to load in Game1.cs
        public List<Enemy> enemyList = new List<Enemy>();

        int currentLevel;

        /// <summary>
        /// Cretes a new instance of a LevelReader, and loads the ements of the first level
        /// </summary>
        public LevelReader()
        {
            this.currentLevel = 0;
            // Get working directory and populate file array
            string workingDirectory = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(workingDirectory);

            // set regular expression to check for only files that end in '.lvl'
            //TODO get help with regex
            Regex reg = new Regex("^.*lvl");

            // check all files in the working directory to see if any end with .lvl,
            // if they do, add them to the list of level files
            foreach (string file in files)
            {
                if (reg.IsMatch(file))
                {
                    levelFiles.Add(file);
                }
            }

            // load items for first level
            if (levelFiles.Count > 0)
            {
                getNextLevel();
            }
            else
            {
                Console.Error.WriteLine("Error: could not find any level files in: " + workingDirectory);
            }
        }

        /// <summary>
        /// Gets the next level
        /// </summary>
        /// <returns></returns>
        public void getNextLevel()
        {
            // get current level from the ArrayList of levels and increment the currentLevel
            string level = (string)levelFiles[currentLevel];
            currentLevel++;

            // Create reader from current level
            StreamReader reader = new StreamReader(File.OpenRead(level));

            // Read the first line, and store it as the background
            this.background = reader.ReadLine();

            // Read the second line, and store it as the song
            this.levelSong = reader.ReadLine();
 
            // The rest of the lines will be enemy placements/appear times
            string line = reader.ReadLine();
            while (line != null)
            {
                string[] output = line.Split(';');

                //First entry will be the enemy type
                string type = output[0];
                
                //Second entry will be the time;
                int appearTime = Convert.ToInt32(output[1]);

                // Third will be the appear x
                int appx = Convert.ToInt32(output[2]);

                // Forth will the the tar x
                int xpos = Convert.ToInt32(output[3]);

                // Fifth will be the tar y
                int ypos = Convert.ToInt32(output[4]);

                // Sixth will be the moveSpeed
                int speed = Convert.ToInt32(output[5]);

                if (type == "Grunt")
                {
                    //Create a blank grunt
                    Grunt grunt = new Grunt();
                    grunt.xPos = appx;
                    grunt.yPos = -10;
                    grunt.tarx = xpos;
                    grunt.tary = ypos;
                    grunt.appearTime = appearTime;
                    grunt.moveSpeed = speed;
                    //TODO figure out weird null exception
                    enemyList.Add(grunt);
                }
                else if (type == "Boss")
                {
                    int health = Convert.ToInt32(output[6]);

                    Boss boss = new Boss(appx,-10,1000,xpos,ypos);
                    boss.appearTime = appearTime;
                    enemyList.Add(boss);
                }
                /*
                 * Additional types go here
                else if
                {
                }
                 */
                else
                {
                    // Do nothing and print error
                    Console.Error.WriteLine("Error unrecognized enemy type: <" + type + ">");
                }
                line = reader.ReadLine();
            }
        }
    }
}