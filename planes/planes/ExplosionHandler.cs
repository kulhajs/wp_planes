using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace planes
{
    class ExplosionHandler
    {
        public List<Explosion> explosions;
        Explosion newExplosion;

        private Vector2 direction = new Vector2(-1,0);
        private Vector2 velocity = new Vector2(100,0);
        
        public ExplosionHandler()
        {
            explosions = new List<Explosion>();
        }

        public void CreateExplosion(string explosionType, Vector2 position, ContentManager contentManager)
        {
            if (explosionType == "small")
                newExplosion = new Explosion(explosionType, new Vector2(6.4f, 6.4f), position);
            else if (explosionType == "normal")
                newExplosion = new Explosion(explosionType, new Vector2(32, 32), position);
            else if (explosionType == "huge")
                newExplosion = new Explosion(explosionType, Vector2.Zero, position);

            newExplosion.LoadContent(contentManager);
            explosions.Add(newExplosion);
        }

        public void Update(GameTimer timer, bool move, bool soundMuted)
        {
            foreach (Explosion e in explosions)
            {
                e.Explode(soundMuted);
                if (move)
                    e.Position += direction * velocity * (float)timer.UpdateInterval.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Explosion e in explosions)
                e.Draw(theSpriteBatch, e.origin, e.Position, Color.White, 0.0f);
        }
    }
}
