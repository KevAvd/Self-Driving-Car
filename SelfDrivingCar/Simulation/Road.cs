using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{
    internal class Road
    {
        Vector2f[] roads =
        {
            new Vector2f(-300,  400),
            new Vector2f(-300, -400),
            new Vector2f(-300,-1200)
        };

        public Vector2f FrontRoad { get => roads[2]; }
        public Vector2f MiddleRoad { get => roads[1]; }
        public Vector2f BackRoad { get => roads[0]; }

        public Road()
        {
            //TODO: Setup sprite
            Globals.SPRITE_ROAD.TextureRect = Globals.ROAD_TEXCOORDS;
            Globals.SPRITE_ROAD.Scale = new Vector2f(Globals.ROAD_WIDTH / (22 * 3), Globals.ROAD_HEIGHT / 32);
        }

        public void Update(AI_Car car)
        {
            if(GameTime.RoadAccu >= Globals.ROAD_TIME)
            {
                Vector2f temp1 = roads[1];
                Vector2f temp2 = roads[2];
                roads[0] -= new Vector2f(0, Globals.ROAD_HEIGHT * 3);
                roads[2] = roads[0];
                roads[1] = temp2;
                roads[0] = temp1;
                GameTime.ResetRoadAccumulator();
                return;
            }
            if (car.Position.Y < roads[1].Y)
            {
                Vector2f temp1 = roads[1];
                Vector2f temp2 = roads[2];
                roads[0] -= new Vector2f(0, Globals.ROAD_HEIGHT * 3);
                roads[2] = roads[0];
                roads[1] = temp2;
                roads[0] = temp1;
            }
            //if (car.Position.Y > roads[0].Y)
            //{
            //    Vector2f temp0 = roads[0];
            //    Vector2f temp1 = roads[1];
            //    roads[2] += new Vector2f(0, Globals.ROAD_HEIGHT * 3);
            //    roads[0] = roads[2];
            //    roads[1] = temp0;
            //    roads[2] = temp1;
            //}
        }

        public void Draw(RenderTarget trgt)
        {
            for (int i = 0; i < roads.Length; i++)
            {
                Globals.SPRITE_ROAD.Position = roads[i];
                trgt.Draw(Globals.SPRITE_ROAD);
            }
        }
    }
}
