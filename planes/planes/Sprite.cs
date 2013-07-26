using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace planes
{
    class Sprite
    {
        public Texture2D texture;
        private Rectangle source;
        private Rectangle size;
        private float scale = 1.0f;
        public Vector2 Position = Vector2.Zero;
        public ContentManager contentManager;

        public float Rotation { get; set; }

        public float X
        {
            get { return this.Position.X; }
            set { this.Position.X = value; }
        }

        public float Y
        {
            get { return this.Position.Y; }
            set { this.Position.Y = value; }
        }

        public Color Color { get; set; }

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                size = new Rectangle(0, 0, (int)(source.Width * scale), (int)(source.Height * scale));
            }
        }

        public Rectangle Source
        {
            get { return this.source; }
            set
            {
                source = value;
                size = new Rectangle(0, 0, source.Width, source.Height);
            }
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            texture = theContentManager.Load<Texture2D>(theAssetName);
            Source = new Rectangle(0, 0, (int)(texture.Width * Scale), (int)(texture.Height * scale));
            size = new Rectangle(0, 0, (int)(texture.Width * Scale), (int)(texture.Height * scale));
        }

        public virtual void Draw(SpriteBatch theSpriteBatch, Vector2 origin, Vector2 position, Color color, float rotation)
        {
            theSpriteBatch.Draw(texture, Position, Source, color, rotation, origin, Scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// float sqrt
        /// </summary>
        public float Fsqrt(float x)
        {
            return (float)Math.Sqrt((double)x);
        }

        /// <summary>
        /// float arc tan
        /// </summary>
        public float FAtan(float x)
        {
            return (float)Math.Atan((double)x);
        }

        /// <summary>
        /// float sinus
        /// </summary>
        public float FSin(float x)
        {
            return (float)Math.Sin((double)x);
        }

        /// <summary>
        /// float cosinus
        /// </summary>
        public float FCos(float x)
        {
            return (float)Math.Cos((double)x);
        }

        /// <summary>
        /// float absolute value
        /// </summary>
        public float FAbs(float x)
        {
            return (float)Math.Abs((double)x);
        }

        /// <summary>
        /// float Pi
        /// </summary>
        public float FPI { get { return (float)Math.PI; } }

    }
}