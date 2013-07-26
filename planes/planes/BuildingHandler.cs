using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace planes
{
    class BuildingHandler
    {
        public List<Building> buildings;
        private Building newBuilding;

        public BuildingHandler()
        {
            buildings = new List<Building>();
        }

        public void CreateBuilding(string buildingType, ContentManager theContentManager)
        {
            newBuilding = new Building(buildingType);
            newBuilding.LoadContent(theContentManager);
            newBuilding.Position = new Vector2(850, 460);
            buildings.Add(newBuilding);
        }

        public void UpdateBuildings(GameTimer timer, Player player)
        {
            foreach (Building b in buildings)
                    b.Update(timer, player);
        }

        public void DrawBuildings(SpriteBatch theSpritebatch)
        {
            if (buildings.Count > 0)
                foreach (Building b in buildings)
                        b.Draw(theSpritebatch);
        }
    }
}
