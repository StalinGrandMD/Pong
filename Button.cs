using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    
    public class Button
    {
        private Texture2D textureNormal;
        private Texture2D textureHover;
        private Texture2D texturePressed;
        private Vector2 position;
        private Rectangle bounds;
        private bool isHovered;
        private bool isPressed;
        

        public Button(Texture2D normal, Texture2D hover, Texture2D pressed, Vector2 position)
        {
            textureNormal = normal;
            textureHover = hover;
            texturePressed = pressed;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, normal.Width, normal.Height);
            bounds.X = Convert.ToInt32(position.X - textureNormal.Width /2);
            bounds.Y = Convert.ToInt32(position.Y - textureNormal.Height /2);
        }

        public void Update(MouseState mouseState)
        {
            isHovered = bounds.Contains(mouseState.Position);
            isPressed = isHovered && mouseState.LeftButton == ButtonState.Pressed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (isPressed)
                spriteBatch.Draw(texturePressed, position,null,Color.White, 0f, new Vector2(texturePressed.Width /2, texturePressed.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            else if (isHovered)
                spriteBatch.Draw(textureHover, position, null, Color.White, 0f, new Vector2(textureHover.Width / 2, textureHover.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(textureNormal, position, null, Color.White, 0f, new Vector2(textureNormal.Width / 2, textureNormal.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }

        public bool IsClicked(MouseState mouseState, MouseState previousMouseState)
        {
            return isHovered && mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
        }
    }
}
