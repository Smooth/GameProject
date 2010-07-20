using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace Prototype1
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameModel : Microsoft.Xna.Framework.DrawableGameComponent //GameComponent opposed(hmm) to DrawableGameComponent is not drawn.
    {

        //the model mesh
        public Model model;
        public Vector3 position;
        public bool isVisible;

        //matrices
        public Matrix init;
        public Matrix world;
        public Matrix view;
        public Matrix projection;

        private float boundingRadius;

        public float BoundingRadius
        {
            get { return this.model.Meshes[0].BoundingSphere.Radius * scale; }
        }


        //Transformations
        public Matrix[] transforms;

        //Scale of the model
        public float scale = 1.0f;

        //modelname
        public String assetName; //Asset = Content that is part of the project.


        public GameModel(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>(assetName);
            base.LoadContent();
        }



        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Draw the model

            if (isVisible)
            {
                // copy parent transforms
                transforms = new Matrix[model.Bones.Count]; //only makes sense if bone count is changed during runtime...so barely ever.
                model.CopyAbsoluteBoneTransformsTo(transforms);

                //do the draw
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects) //XNA is completely based on shaders. That's why standard shaders (BasicEffects) are implemented so that you can draw graphics
                    {
                        //default lighting
                        effect.EnableDefaultLighting();
                        //set world matrix
                        effect.World = transforms[mesh.ParentBone.Index] * world;
                        //set view/camera
                        effect.View = view;
                        //projection
                        effect.Projection = projection;
                    }

                    //draw mesh
                    mesh.Draw();
                }
            }

            base.Draw(gameTime);
        }
    }
} //end namespace Shooter3D