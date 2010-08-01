using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace GameProject
{
    class Fireball : Sprite
    {
        const int MAX_DISTANCE = 500;

        public bool Visible = false;

        Vector2 mStartPosition;
        Vector2 mSpeed;
        Vector2 mDirection;

        const string FIREBALL_ASSETNAME = "Actors\\2D\\Fireball";


        //public void Initialize(Game1 game, Matrix view, Matrix projection)
        //{
        //    //create a rocket
        //    Ship rocket = new Ship(game);
        //    rocket.view = view;
        //    rocket.projection = projection;
        //    rocket.world = Matrix.Identity;
        //    rocket.isVisible = true;
        //    game.Components.Add(rocket);
        //}

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, FIREBALL_ASSETNAME);
            Scale = 1.0f;
        }

        public void Update(GameTime theGameTime)
        {
            if (Vector2.Distance(mStartPosition, Position) > MAX_DISTANCE)
            {
                Visible = false;
            }

            if (Visible == true)
            {
                base.Update(theGameTime, mSpeed, mDirection);
            }
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (Visible == true)
            {
                base.Draw(theSpriteBatch);
            }
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            Position = theStartPosition;
            mStartPosition = theStartPosition;
            mSpeed = theSpeed;
            mDirection = theDirection;
            Visible = true;
        }


    }
}
