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
    class IntersectionHandler
    {
        Random random = new Random();

        Rectangle playerRectangle;
        Rectangle enemyRectangle;
        Rectangle bulletRectangle;
        Rectangle powerupRectangle;
        Rectangle bombRectangle;
        Rectangle buildingRectangle;

        private bool bulletRemoved = false;
        private bool bombRemoved = false;

        public void HandlePowerupIntersections(PowerupHandler pu, Player player)
        {
            playerRectangle = new Rectangle((int)player.X - player.W / 2, (int)player.Y - player.H / 2, player.W, player.H);
            foreach (Powerup p in pu.powerups)
            {
                powerupRectangle = new Rectangle((int)p.X, (int)p.Y, p.W, p.H);
                if (playerRectangle.Intersects(powerupRectangle))
                {
                    if (p.Type == "health")
                    {
                        if (player.Hitpoints + 5 < player.MaxHealth)
                            player.Hitpoints += 5;
                        else
                        {
                            if (player.healthPowerups.Count < 5)
                            {
                                p.Source = new Rectangle(0, 0, p.W, p.H);
                                p.Position = new Vector2(20 + player.healthPowerups.Count * 40, 20);
                                p.Scale = 2f;
                                player.healthPowerups.Add(p);
                            }
                            else
                                player.Hitpoints = player.MaxHealth;
                        }
                    }
                    else if (p.Type == "ammo")
                    {
                        if (player.Ammo + 50 < player.MaxAmmo)
                            player.Ammo += 50;
                        else
                        {
                            if (player.ammoPowerups.Count < 5)
                            {
                                p.Source = new Rectangle(0, 0, p.W, p.H);
                                p.Position = new Vector2(106 + player.ammoPowerups.Count * 18, 335);
                                p.Scale = 0.75f;
                                player.ammoPowerups.Add(p);
                            }
                            else
                                player.Ammo = player.MaxAmmo;
                        }
                    }
                    else if (p.Type == "ten")
                    {
                        //player.score.AddPoints(10);
                    }
                    else if (p.Type == "bomb" && player.AvailibleBombs + 1 <= 2)
                    {
                        player.AvailibleBombs += 1;
                    }

                    pu.powerups.Remove(p);
                    break;
                }
            }
        }

        public void HandleBulletIntersections(EnemyHandler eh, Player p, ContentManager contentManager)
        {

            #region PLAYERS_BULETS
            foreach (Bullet b in p.bullets)
            {
                bulletRectangle = new Rectangle((int)b.X, (int)b.Y, 3, 1);
                foreach (Enemy e in eh.EnemiesIter())
                {
                    if (e.IsAlive)
                    {
                        enemyRectangle = new Rectangle((int)e.X, (int)e.Y - 100, e.W, e.H);
                        if (enemyRectangle.Intersects(bulletRectangle))
                        {
                            e.Hitpoints -= 1;
                            e.explosionHandler.CreateExplosion("small", new Vector2(b.X - 75, b.Y), contentManager);
                            p.bullets.Remove(b);
                            bulletRemoved = true;
                        }
                    }
                }
                if (bulletRemoved)
                {
                    bulletRemoved = false;
                    break;
                }
            }
            #endregion

            #region ENEMIES_BULLETS
            playerRectangle = new Rectangle((int)p.X - p.W / 2, (int)p.Y - p.H / 2, p.W, p.H);
            foreach (Enemy e in eh.EnemiesIter())
            {
                foreach (Bullet b in e.bullets)
                {
                    bulletRectangle = new Rectangle((int)b.X, (int)b.Y, 3, 1);
                    if (bulletRectangle.Intersects(playerRectangle))
                    {
                        p.Hitpoints -= 1;
                        if (p.IsAlive)
                            e.explosionHandler.CreateExplosion("small", new Vector2(b.X - random.Next(5, 20), b.Y), contentManager);
                        e.bullets.Remove(b);
                        break;
                    }
                }
            }
            #endregion
        }

        public void HandlePlanesInterestion(EnemyHandler eh, Player p)
        {
            playerRectangle = new Rectangle((int)p.X, (int)p.Y, p.W, p.H);
            foreach (Enemy e in eh.EnemiesIter())
                if (e.IsAlive)
                {
                    enemyRectangle = new Rectangle((int)e.X, (int)e.Y - 100, e.W, e.H);
                    if (playerRectangle.Intersects(enemyRectangle))
                    {
                        e.Hitpoints = 0;
                        p.Hitpoints -= 10;
                    }
                }
        }

        //public void HandleBuildingIntersection(Player p, BuildingHandler bh)
        //{
        //    playerRectangle = new Rectangle((int)p.X + p.W / 2, (int)p.Y + p.H / 4, p.W / 4, p.H / 2);
        //    foreach (Building b in bh.buildings)
        //    {
        //        buildingRectangle = new Rectangle((int)b.X + 5, (int)b.Y - b.H + 10, b.W - 10, b.H);
        //        if (playerRectangle.Intersects(buildingRectangle))
        //        {
        //            b.Damaged = true;
        //            p.Hitpoints = 0;
        //            p.Crash = true;
        //        }
        //    }
        //}

        //public void HandleBombsIntersection(Player p, BuildingHandler bh, ContentManager theContentManager)
        //{
        //    if (p.bombs.Count > 0)
        //    {
        //        foreach (Bomb b in p.bombs)
        //        {
        //            bombRectangle = new Rectangle((int)b.X, (int)b.Y, 20, 20); //TODO: width, height if needed
        //            foreach (Building building in bh.buildings)
        //            {
        //                buildingRectangle = new Rectangle((int)building.X, (int)building.Y - building.H + 10, building.W, building.H);
        //                if (bombRectangle.Intersects(buildingRectangle))
        //                {
        //                    building.Damaged = true;
        //                    p.score.AddPoints(50);
        //                    p.eh.CreateExplosion("huge", new Vector2(b.X - 48, b.Y - 16), theContentManager);
        //                    p.bombs.Remove(b);
        //                    bombRemoved = true;
        //                }
        //            }
        //            if (bombRemoved)
        //            {
        //                bombRemoved = false;
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
