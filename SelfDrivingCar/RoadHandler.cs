using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{
    internal class RoadHandler
    {
        Road[] roads = new Road[3];
        const float width = 600;
        const float height = 800;
        public RoadHandler() 
        {
            roads[0] = new Road(new Vector2f(-300,400));    
            roads[1] = new Road(new Vector2f(-300,-400));    
            roads[2] = new Road(new Vector2f(-300,-1200));    
        }

        public void Update(Car car)
        {
            if(car.Position.Y < roads[1].Position.Y)
            {
                Road temp1 = roads[1];
                Road temp2 = roads[2];
                roads[0].Position -= new Vector2f(0, height * 3);
                roads[2] = roads[0];
                roads[1] = temp2;
                roads[0] = temp1;
            }
            if (car.Position.Y > roads[0].Position.Y)
            {
                Road temp0 = roads[0];
                Road temp1 = roads[1];
                roads[2].Position += new Vector2f(0, height * 3);
                roads[0] = roads[2];
                roads[1] = temp0;
                roads[2] = temp1;
            }
            foreach (Road road in roads) { road.Update(); }
        }

        public void Draw(RenderTarget trgt)
        {
            foreach(Road road in roads)
            {
                road.Draw(trgt);
            }
        }
    }
}
