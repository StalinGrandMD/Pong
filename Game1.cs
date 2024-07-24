using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Diagnostics;

namespace Pong
{
    public class Game1 : Game
    {
        Texture2D Player1Texture;
        Vector2 Player1Position;
        Texture2D Player2Texture;
        Vector2 Player2Position;
        Texture2D ballTexture;
        Vector2 ballPosition;
        Rectangle smallRectangle;
        Rectangle big1Rectangle;
        Rectangle big2Rectangle;
        Texture2D pixel;
        bool plus = true;
        bool MOVE=false;
        bool start=true;
        bool angle=true;
        float ballSpeed;
        float PlayerSpeed;
        SpriteFont font;
        int Player1score;
        int Player2score;
        string score;
        Texture2D buttonNormal;
        Texture2D buttonHover;
        Texture2D buttonPressed;
        Texture2D botbuttonNormal;
        Texture2D botbuttonPressed;
        Texture2D botbuttonHover;
        Button myButton;
        Button botbutton;
        bool buttonVisible;
        MouseState previousMouseState;
        SoundEffect pedal;
        SoundEffect wall;
        SoundEffect Pscore;
        bool botactive;
        


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 640;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();
            Player1Position = new Vector2(0,_graphics.PreferredBackBufferHeight / 2);
            Player2Position = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            PlayerSpeed = 250f;
            ballSpeed = 4;
            Player1score = 0;
            Player2score = 0;
            buttonVisible = true;
            botactive = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Player1Texture = Content.Load<Texture2D>("PlayerWhite");
            Player2Texture = Content.Load<Texture2D>("PlayerWhite");
            ballTexture = Content.Load<Texture2D>("PongWhite");
            smallRectangle = new Rectangle(_graphics.PreferredBackBufferWidth /2, _graphics.PreferredBackBufferHeight /2,ballTexture.Width,ballTexture.Height);
            big1Rectangle = new Rectangle(0, _graphics.PreferredBackBufferHeight / 2, Player1Texture.Width,Player1Texture.Height);
            big2Rectangle = new Rectangle(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2, Player2Texture.Width, Player2Texture.Height);
            pixel = new Texture2D(GraphicsDevice, 1,1 );
            pixel.SetData<Color>(new Color[] { Color.White });
            font = Content.Load<SpriteFont>("score");
            buttonNormal = Content.Load<Texture2D>("buttonNormal");
            buttonHover = Content.Load<Texture2D>("buttonHover");
            buttonPressed = Content.Load<Texture2D>("buttonPressed");
            botbuttonNormal = Content.Load<Texture2D>("botbuttonNo");
            botbuttonHover = Content.Load<Texture2D>("botbuttonNo");
            botbuttonPressed = Content.Load<Texture2D>("botbuttonYes");
            myButton = new Button(buttonNormal, buttonHover, buttonPressed, new Vector2(_graphics.PreferredBackBufferWidth - 160, _graphics.PreferredBackBufferHeight / 2 + 100));
            botbutton = new Button(botbuttonNormal, botbuttonHover, botbuttonPressed, new Vector2(_graphics.PreferredBackBufferWidth / 4, _graphics.PreferredBackBufferHeight / 2 + 100));
            pedal = Content.Load<SoundEffect>("Pongpedal");
            wall = Content.Load<SoundEffect>("Pongwall");
            Pscore = Content.Load<SoundEffect>("Pongscore");
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState currentMouseState = Mouse.GetState();
            myButton.Update(currentMouseState);
            botbutton.Update(currentMouseState);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            


            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.PageDown)){MOVE = false;}
            if (kstate.IsKeyDown(Keys.PageUp)) { MOVE = true; }
            // Player 1
            if (kstate.IsKeyDown(Keys.W))
            {
                Player1Position.Y -= PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Keys.S))
            {
                Player1Position.Y += PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Player1Position.Y > _graphics.PreferredBackBufferHeight - Player1Texture.Height / 2)
            {
                Player1Position.Y = _graphics.PreferredBackBufferHeight - Player1Texture.Height / 2;
            }
            else if (Player1Position.Y < Player1Texture.Height / 2)
            {
                Player1Position.Y = Player1Texture.Height / 2;
            }
            // Player 2
            if (kstate.IsKeyDown(Keys.Up))
            {
                Player2Position.Y -= PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Keys.Down))
            {
                Player2Position.Y += PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (Player2Position.Y > _graphics.PreferredBackBufferHeight - Player2Texture.Height / 2)
            {
                Player2Position.Y = _graphics.PreferredBackBufferHeight - Player2Texture.Height / 2;
            }
            else if (Player2Position.Y < Player2Texture.Height / 2)
            {
                Player2Position.Y = Player2Texture.Height / 2;
            }
            //
            smallRectangle.X = Convert.ToInt32(ballPosition.X - ballTexture.Width /2);
            smallRectangle.Y = Convert.ToInt32(ballPosition.Y - ballTexture.Height /2);
            big1Rectangle.Y = Convert.ToInt32(Player1Position.Y - Player1Texture.Height /2);
            big1Rectangle.X = Convert.ToInt32(Player1Position.X);
            big2Rectangle.Y = Convert.ToInt32(Player2Position.Y - Player2Texture.Height / 2);
            big2Rectangle.X = Convert.ToInt32(Player2Position.X - Player2Texture.Width);
            if (kstate.IsKeyDown(Keys.Home))
            {
                botactive = true;
            }
            if (kstate.IsKeyDown (Keys.End)) 
            {
                botactive = false;
            }
            if (botactive) { Player2Position.Y = ballPosition.Y; }
            
            //
            // Ball

            if (MOVE)
            {
                if (start == true)
                {

                    if (big1Rectangle.Intersects(smallRectangle))
                    {
                        pedal.Play();
                        start = false;

                    }
                    else
                    {
                        ballPosition.X -= ballSpeed;

                    }
                    
                    
                }
                else
                {
                    if (big2Rectangle.Intersects(smallRectangle))
                    {
                        pedal.Play();
                        start = true;
                        
                        
                    }
                    else
                    {

                        ballPosition.X += ballSpeed;


                    }
                    

                }
                if (smallRectangle.Y >= _graphics.PreferredBackBufferHeight - 25)
                {
                    wall.Play();
                    plus = false;
                }
                if (smallRectangle.Y <= 0)
                {
                    wall.Play();
                    plus = true;
                }
                if (angle)
                {
                    if (plus == true) { ballPosition.Y += ballSpeed - 1; }
                    if (plus == false) { ballPosition.Y -= ballSpeed - 1; }
                }
                if (smallRectangle.Left <= 0 )
                {
                    Pscore.Play();
                    MOVE = false;
                    ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
                    Player1Position = new Vector2(0, _graphics.PreferredBackBufferHeight / 2);
                    Player2Position = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
                    Player1score += 1;
                    start = false;
                    buttonVisible = true;
                }
                if (smallRectangle.Right >= _graphics.PreferredBackBufferWidth)
                {
                    Pscore.Play();
                    MOVE = false;
                    ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
                    Player1Position = new Vector2(0, _graphics.PreferredBackBufferHeight / 2);
                    Player2Position = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
                    Player2score += 1;
                    start = true;
                    buttonVisible = true;
                }
                
            }
            //
            score = Convert.ToString(Player1score)+":"+Convert.ToString(Player2score);
            //
            if (myButton.IsClicked(currentMouseState, previousMouseState))
            {
                // Handle button click
                MOVE=true;
                buttonVisible = false;
            }
            if (botbutton.IsClicked(currentMouseState, previousMouseState))
            {
                if (botactive == true)
                {
                    botactive = false;
                    botbutton = new Button(botbuttonNormal, botbuttonHover, botbuttonPressed, new Vector2(_graphics.PreferredBackBufferWidth / 4, _graphics.PreferredBackBufferHeight / 2 + 100));
                }
                else
                {
                    botactive = true;
                    botbutton = new Button(botbuttonPressed, botbuttonPressed, botbuttonNormal, new Vector2(_graphics.PreferredBackBufferWidth / 4, _graphics.PreferredBackBufferHeight / 2 + 100));
                }
                
            }
            previousMouseState = currentMouseState;
            //
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(Player1Texture, Player1Position,null, Color.White,0f,new Vector2(0,Player1Texture.Height /2),Vector2.One,SpriteEffects.None,0f);
            _spriteBatch.Draw(Player2Texture, Player2Position, null, Color.White, 0f, new Vector2(Player2Texture.Width, Player2Texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            _spriteBatch.Draw(ballTexture,ballPosition ,null,Color.White,0f,new Vector2(ballTexture.Width/2,ballTexture.Height/2),Vector2.One, SpriteEffects.None, 0f);
            //_spriteBatch.Draw(pixel, smallRectangle, Color.White);
            //_spriteBatch.Draw(pixel, big1Rectangle, Color.Green);
            //_spriteBatch.Draw(pixel, big2Rectangle, Color.Lime);
            _spriteBatch.DrawString(font, score,new Vector2(_graphics.PreferredBackBufferWidth / 2 - 40, _graphics.PreferredBackBufferHeight / 4 - 100), Color.White);
            if (buttonVisible == true)
            {
                myButton.Draw(_spriteBatch);
                botbutton.Draw(_spriteBatch);
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
