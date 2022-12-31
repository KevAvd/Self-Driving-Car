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

        AABB leftAABB = new AABB();
        AABB rightAABB = new AABB();

        public Vector2f FrontRoad { get => roads[2]; }
        public Vector2f MiddleRoad { get => roads[1]; }
        public Vector2f BackRoad { get => roads[0]; }
        internal AABB LeftAABB { get => leftAABB; }
        internal AABB RightAABB { get => rightAABB; }

        public Road()
        {
            //TODO: Setup sprite
            Globals.SPRITE_ROAD.TextureRect = Globals.ROAD_TEXCOORDS;
            Globals.SPRITE_ROAD.Scale = new Vector2f(Globals.ROAD_WIDTH / (22 * 3), Globals.ROAD_HEIGHT / 32);
            UpdateAABBs();
        }

        public void Update(AI_Car car)
        {
            if (GameTime.RoadAccu >= Globals.ROAD_TIME)
            {
                Vector2f temp1 = roads[1];
                Vector2f temp2 = roads[2];
                roads[0] -= new Vector2f(0, Globals.ROAD_HEIGHT * 3);
                roads[2] = roads[0];
                roads[1] = temp2;
                roads[0] = temp1;
                GameTime.ResetRoadAccumulator();
                UpdateAABBs();
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
                UpdateAABBs();
            }
        }

        public void UpdateAABBs()
        {
            //Update left AABB
            leftAABB.p1 = FrontRoad + new Vector2f(-100, 0);
            leftAABB.p2 = FrontRoad;
            leftAABB.p3 = BackRoad + new Vector2f(0, Globals.ROAD_HEIGHT);
            leftAABB.p4 = BackRoad + new Vector2f(-100, Globals.ROAD_HEIGHT);

            //Update right AABB
            rightAABB.p1 = FrontRoad + new Vector2f(Globals.ROAD_WIDTH, 0);
            rightAABB.p2 = FrontRoad + new Vector2f(Globals.ROAD_WIDTH + 100, 0);
            rightAABB.p3 = BackRoad + new Vector2f(Globals.ROAD_WIDTH +100, Globals.ROAD_HEIGHT);
            rightAABB.p4 = BackRoad + new Vector2f(Globals.ROAD_WIDTH, Globals.ROAD_HEIGHT);
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
