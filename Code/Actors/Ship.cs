using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace GameProject
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Ship : GameModel
    {
        public Ship(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            assetName = "Models\\rocket";
            scale = 200.0f; //uniform scale because of bad modelsize.
            //int the model
            init = Matrix.CreateScale(scale) * Matrix.CreateRotationY((float)Math.PI); //PiOver2 and PiOver4 = divisions of Pi (MathHelper.Pi)
            world = init;

            base.Initialize();
        }

        public void Explode()
        {
            world = init;
            this.position = new Vector3(0.0f, 0.0f, 0.0f);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //ship control
            KeyboardState kBState;
            GamePadState gPState;

            kBState = Keyboard.GetState();
            gPState = GamePad.GetState(PlayerIndex.One);

            if (kBState.IsKeyDown(Keys.Left) ||
                gPState.DPad.Left == ButtonState.Pressed ||
                gPState.ThumbSticks.Left.X <= -0.2)
            {
                this.position.X -= (float)gameTime.ElapsedGameTime.Milliseconds * 0.1f;
                world = init * Matrix.CreateRotationZ(MathHelper.PiOver4) * Matrix.CreateTranslation(this.position);
            }
            else if (kBState.IsKeyDown(Keys.Right) ||
                     gPState.DPad.Right == ButtonState.Pressed ||
                gPState.ThumbSticks.Left.X >= 0.2)
            {
                this.position.X += (float)gameTime.ElapsedGameTime.Milliseconds * 0.1f;
                world = init * Matrix.CreateRotationZ(-MathHelper.PiOver4) * Matrix.CreateTranslation(this.position);
            }
            else
            {
                world = init * Matrix.CreateTranslation(this.position);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}