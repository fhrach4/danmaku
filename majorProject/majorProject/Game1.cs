using System;
using System.Collections;
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
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public Texture2D singlePix;
        //####Constants
        //##Human Constants
        //Speed constantes
        protected int MAX_HUMAN_SPEED = 7;
        //Animation constants
        protected int MAX_HUMAN_FRAMES = 10;
        protected int MIN_HUMAN_FRAMES = 0;
        protected int HUMAN_SPRITE_WIDTH = 34;
        protected int HUMAN_SPRITE_HEIGHT = 38;
        protected int HUMAN_NEUTRAL_FRAME = 5;
        //Human position constants
        protected int HUMAN_START_X = 200;
        protected int HUMAN_START_Y = 200;
        //Globals
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Effects
        public ArrayList explosionList = new ArrayList();
        Texture2D explosionTexture;

        //player
        Player human;

        //enemy list
        public ArrayList enemyList;
        public ArrayList removeList = new ArrayList();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            // Define Window here
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Window.Title = ("Major Project");
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create Level Reader
            LevelReader reader = new LevelReader(); 
            //create player
            singlePix = Content.Load<Texture2D>("singlePix");
            Texture2D enemyText = Content.Load<Texture2D>("Enemy1");
            Texture2D shotTexture = Content.Load<Texture2D>("shot1");
            Texture2D humanTexture = Content.Load<Texture2D>("player");
            AnimatedSprite humanAnimatedTexture = new AnimatedSprite(humanTexture, HUMAN_NEUTRAL_FRAME, HUMAN_NEUTRAL_FRAME, MAX_HUMAN_FRAMES, HUMAN_SPRITE_WIDTH, HUMAN_SPRITE_HEIGHT);
            human = new Player(humanAnimatedTexture, shotTexture, HUMAN_START_X, HUMAN_START_Y, MAX_HUMAN_SPEED);

            //create enemies
            enemyList = new ArrayList();
            //for (int i = 0; i <= 800; i = i + 40)
            //{
            //    Grunt enemy = new Grunt(enemyText, 34, 38, i, i / 2, 20);
            //    enemyList.Add(enemy);
            //}

            Grunt enemy = new Grunt(enemyText, 34, 38, 200, 200, 20, 0.1);
            enemyList.Add(enemy);

            //Load effects
            explosionTexture = Content.Load<Texture2D>("explosion");
    
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            updateEnemies();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            //draw enemies
            foreach (Enemy enemy in enemyList)
            {
                enemy.draw(spriteBatch);
            }
            //handle player movement
            Vector2 humanPos = human.updateState(gameTime);
            human.drawShots(spriteBatch);
            human.sprite.draw(spriteBatch, humanPos);

            //draw effects and remove players/enemies
            updateEffects(spriteBatch);
            removeEnemies(spriteBatch);

            //hit box for debugging
            spriteBatch.Draw(singlePix, human.hitBox, Color.Red);
            // hit box for enemies
            foreach(Enemy enemy in enemyList)
            {
                //spriteBatch.Draw(singlePix, enemy.hitBox, Color.Yellow);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void updateEnemies()
        {
            // check to see if each enemy is alive
            foreach (Enemy enemy in enemyList)
            {
                enemy.update(human);
                if (!enemy.alive)
                {
                    removeList.Add(enemy);
                }
            }
        }

        protected void removeEnemies(SpriteBatch batch)
        {
            //remove each enemy in the remove list from the main enemy list
            foreach (Enemy enemy in removeList)
            {
                //create a new explosion and add it to the explosion list
                Expolsion exp = new Expolsion(explosionTexture, 128, 128, 20);
                explosionList.Add(exp);
                enemy.die(exp, batch);

                enemyList.Remove(enemy);
            }

            removeList.Clear();
        }

        // update all effects on screen
        protected void updateEffects(SpriteBatch batch)
        {
            ArrayList expRemoveList = new ArrayList();
            foreach (Expolsion exp in explosionList)
            {
                exp.update();
                exp.draw(batch);
                if (exp.finished)
                {
                    expRemoveList.Add(exp);
                }
            }

            foreach (Expolsion exp in expRemoveList)
            {
                explosionList.Remove(exp);
            }
        }
    }
}
