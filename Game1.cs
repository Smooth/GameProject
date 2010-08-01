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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace GameProject
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //--Member-Declarations--------------------------------------

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle viewportRect;
        //SpriteFont debugFont;
        //Vector2 debugTextPosition = new Vector2(0.02f, 0.02f);
        String debugString;

        Background2D background;

        // Input state handler
        KeyboardState kBState;
        GamePadState  gPState;

        //view/camera
        Matrix view;
        Vector3 cameraPosition;
        Vector3 cameraLookAt;

        //projection
        Matrix projection;
        float aspectRatio;

        
        //Sprite player;

        Wizard mWizardSprite;
        Texture2D mCatCreature;
        const string CAT_ASSETNAME = "Background\\CatCreature";
        int mAlphaValue = 1;
        int mFadeIncrement = 3;
        double mFadeDelay = .01;

        Texture2D mHealthBar;
        const string HEALTH_ASSETNAME = "Background\\HealthBar";
        int mCurrentHealth = 100;

        //Background textures for the various screens in the game
        Texture2D mControllerDetectScreenBackground;
        Texture2D mTitleScreenBackground;

        enum ScreenState
        {
            ControllerDetect,
            Title
        }
        //The current screen state
        ScreenState mCurrentScreen;

        ////Screen State variables to indicate what is the current screen
        //bool mIsControllerDetectScreenShown;
        //bool mIsTitleScreenShown;

        //The index of the Player One controller
        PlayerIndex mPlayerOne;



        //--Constructors---------------------------------------------

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            //Initialize screen size to an ideal resolution for the XBox 360
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
        }

        //--Overridden-Functions-------------------------------------

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //drawable area of the game screen.
            viewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            // Create the view frustum
            //XNA uses a right handed coordinate system. in contrast to directX! similar to openGL!!!
            //set the camera
            cameraPosition = new Vector3(0.0f, 200.0f, 0.01f);
            //set lookAt
            cameraLookAt = new Vector3(0.0f, 0.0f, -60.0f);
            //create view
            view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);

            //calc aspect ratio
            aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width /
                                 graphics.GraphicsDevice.Viewport.Height;

            //create projection
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.001f, 1000.0f);



            //mWizardSprite = new Wizard(this, view, projection);
            mWizardSprite = new Wizard();
          
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //-----------------------------------------------------------

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            mWizardSprite.LoadContent(this.Content);

            ContentManager aLoader = new ContentManager(this.Services);
            aLoader.RootDirectory = "Content";
            mCatCreature = aLoader.Load<Texture2D>(CAT_ASSETNAME) as Texture2D;
            mHealthBar = aLoader.Load<Texture2D>(HEALTH_ASSETNAME) as Texture2D;
            // Load a certain font type for debugging output
            //debugFont = Content.Load<SpriteFont>("Fonts\\Debug");

            // Load a string from an XML file for testing
            //debugString = Content.Load<String>("Management\\GameFlow");

            //-----------------------------------------------------------

            // Create background object and add images
            background = new Background2D(this.GraphicsDevice.Viewport);
            background.AddBackground("Background\\Background01");
            background.AddBackground("Background\\Background02");
            background.AddBackground("Background\\Background03");
            background.AddBackground("Background\\Background04");
            background.AddBackground("Background\\Background05");

            // Load background images
            background.LoadContent(this.Content);

            //Load the screen backgrounds
            mControllerDetectScreenBackground = aLoader.Load<Texture2D>("Background\\ControllerDetectScreen") as Texture2D;
            mTitleScreenBackground = aLoader.Load<Texture2D>("Background\\TitleScreen") as Texture2D;

            ////Initialize the screen state variables
            //mIsTitleScreenShown = false;
            //mIsControllerDetectScreenShown = true;

            //Initialize the current screen state to the screen we want to display first
            mCurrentScreen = ScreenState.ControllerDetect;

            //-----------------------------------------------------------

            // Create player
            //player = new Sprite();

            // Load player image
            //player.LoadContent(this.Content, "Actors\\2D\\Player");

            

            //-----------------------------------------------------------

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mWizardSprite.Update(gameTime);

            kBState = Keyboard.GetState();
            //foreach (Keys key in kBState.GetPressedKeys())
            //{
            //    switch (key)
            //    {
            //        case Keys.Left:
            //            background.Update(gameTime, 150, Background2D.HorizontalScrollDirection.Right);
            //            break;
            //        case Keys.Right:
            //            background.Update(gameTime, 150, Background2D.HorizontalScrollDirection.Left);
            //            break;
            //        default:
            //            break;
            //    }
            //}

            Vector2 direction = new Vector2(-gPState.ThumbSticks.Left.X, gPState.ThumbSticks.Left.Y);
            gPState = GamePad.GetState(PlayerIndex.One);
            if (kBState.IsKeyDown(Keys.Left) ||
                gPState.DPad.Left == ButtonState.Pressed)
            {
                direction.X = 1;
                direction.Y = 0;
            }
            else if (kBState.IsKeyDown(Keys.Right) ||
                     gPState.DPad.Right == ButtonState.Pressed)
            {
                direction.X = -1;
                direction.Y = 0;
            }
            else if (kBState.IsKeyDown(Keys.Up) ||
                     gPState.DPad.Up == ButtonState.Pressed)
            {
                direction.X = 0;
                direction.Y = 1;
            }
            else if (kBState.IsKeyDown(Keys.Down) ||
                     gPState.DPad.Down == ButtonState.Pressed)
            {
                direction.X = 0;
                direction.Y = -1;
            }

            //background.Update(gameTime, 150, direction);

            // TODO: Add your update logic here
            mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;

            if (mFadeDelay <= 0)
            {
                mFadeDelay = .01;
                mAlphaValue += mFadeIncrement;

                if (mAlphaValue >= 255 || mAlphaValue <= 0)
                {
                    mFadeIncrement *= -2;
                    //mFadeIncrement *= -1;
                }
            }

            KeyboardState mKeys = Keyboard.GetState();

            if (mKeys.IsKeyDown(Keys.E) == true)
            {
                mCurrentHealth += 1;
            }

            //If the Down Arrowis pressed, decrease the Health bar

            if (mKeys.IsKeyDown(Keys.D) == true)
            {
                mCurrentHealth -= 1;
            }

            mCurrentHealth = (int)MathHelper.Clamp(mCurrentHealth, 0, 100);

            //Update method associated with the current screen
            switch (mCurrentScreen)
            {
                case ScreenState.ControllerDetect:
                    {
                        UpdateControllerDetectScreen();
                        break;
                    }
                case ScreenState.Title:
                    {
                        UpdateTitleScreen();
                        break;
                    }
            }
                
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            // Call spriteBatch.Begin before you start drawing content.
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            // Draw debug output on screen.
            //spriteBatch.DrawString(debugFont, debugString, new Vector2(debugTextPosition.X * viewportRect.Width, debugTextPosition.Y * viewportRect.Height), Color.Crimson);

            // Draw the background


            // Draw the main character
            //player.Draw(spriteBatch);

            //Based on the screen state variables, call the

            //Draw method associated with the current screen

            switch (mCurrentScreen)
            {
                case ScreenState.ControllerDetect:
                    {
                        DrawControllerDetectScreen();
                        break;
                    }
                case ScreenState.Title:
                    {

                        background.Draw(spriteBatch);

                        mWizardSprite.Draw(this.spriteBatch);

                        spriteBatch.Draw(mCatCreature, new Rectangle(1000, 350, mCatCreature.Width, mCatCreature.Height),
                            new Color(255, 255, 255, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));

                        //Draw the negative space for the health bar
                        spriteBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,
                        30, mHealthBar.Width, 60), new Rectangle(0, 0, mHealthBar.Width, 60), Color.Gray);

                        //Draw the current health level based on the current health
                        spriteBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,
                        30, (int)(mHealthBar.Width * ((double)mCurrentHealth / 100)), 60), new Rectangle(0, 61, mHealthBar.Width, 60), Color.Red);

                        //Draw the box around the health bar
                        spriteBatch.Draw(mHealthBar, new Rectangle(this.Window.ClientBounds.Width / 2 - mHealthBar.Width / 2,
                              30, mHealthBar.Width, 60), new Rectangle(0, 0, mHealthBar.Width, 60), Color.White);
                        //DrawTitleScreen();
                        break;
                    }
            }


            // Call spriteBatch.End after all drawing is done.
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateControllerDetectScreen()
        {
            //Poll all the gamepads (and the keyboard) to check to see
            //which controller will be the player one controller
            for (int aPlayer = 0; aPlayer < 4; aPlayer++)
            {
                if (GamePad.GetState((PlayerIndex)aPlayer).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.A) == true)
                {
                    mPlayerOne = (PlayerIndex)aPlayer;
                    mCurrentScreen = ScreenState.Title;
                    return;
                }
            }
        }

        private void UpdateTitleScreen()
        {
            //Move back to the Controller detect screen if the player moves
            //back (using B) from the Title screen (this is typical game behavior
            //and is used to switch to a new player one controller)
            if (GamePad.GetState(mPlayerOne).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.B) == true)
            {
                mCurrentScreen = ScreenState.ControllerDetect;
                return;
            }
        }

        private void DrawControllerDetectScreen()
        {
            //Draw all of the elements that are part of the Controller detect screen
            spriteBatch.Draw(mControllerDetectScreenBackground, Vector2.Zero, Color.White);
        }

        private void DrawTitleScreen()
        {   //Draw all of the elements that are part of the Title screen
            spriteBatch.Draw(mTitleScreenBackground, Vector2.Zero, Color.White);
        }









        //--Public-Functions-----------------------------------------

        //--Private-Functions----------------------------------------
    }
}
