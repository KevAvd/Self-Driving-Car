using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{
    internal class CarHandler
    {
        List<Traffic_Car> traffic = new List<Traffic_Car>();
        List<AI_Car> ai_cars = new List<AI_Car>();
        Road road = new Road();
        int focused_ai = 0;

        public AI_Car Focused_AI { get => ai_cars[focused_ai]; }

        public CarHandler()
        {
            Globals.SPRITE_CAR.Origin = new Vector2f(12 / 2, 16 / 2);
            Globals.SPRITE_CAR.Scale = new Vector2f(Globals.CAR_WIDTH / 12, Globals.CAR_HEIGHT / 16);
        }

        public void Update()
        {
            for(int i = traffic.Count - 1; i >= 0; i--)
            {
                //Update traffic
                traffic[i].Update();

                //Remove traffic when outside of road range
                if (traffic[i].Position.Y > road.BackRoad.Y + Globals.ROAD_HEIGHT / 2 || traffic[i].Position.Y < road.FrontRoad.Y)
                {
                    traffic.Remove(traffic[i]);
                    GenerateTraffic(1);
                }
            }
            for(int i = 0; i < ai_cars.Count; i++)
            {
                if (ai_cars[i].Dead) { continue; }

                //Update controls
                ai_cars[i].Forwards = Inputs.IsPressed(Keyboard.Key.W);
                ai_cars[i].Left = Inputs.IsPressed(Keyboard.Key.A);
                ai_cars[i].Backwards = Inputs.IsPressed(Keyboard.Key.S);
                ai_cars[i].Right = Inputs.IsPressed(Keyboard.Key.D);

                //Update car
                ai_cars[i].Update();

                //Kill car if it leave road
                if (ai_cars[i].Position.X > 300 || ai_cars[i].Position.X < -300) { ai_cars[i].Dead = true; }

                //Reset sensors
                ai_cars[i].ResetSensor();
                
                //Sensors check road limit
                for(int j = 0; j < ai_cars[i].Rays.Length; j++)
                {
                    if (ai_cars[i].Rays[j].p2.X > 300 || ai_cars[i].Rays[j].p2.X < -300)
                        ai_cars[i].Sensor[j] = 1;
                }

                //Handle collisions
                for (int j = 0; j < traffic.Count; j++)
                {
                    if (CollisionDetection.AABB_AABB(ai_cars[i].AABB, traffic[j].AABB)) 
                        ai_cars[i].Dead = true;
                    for (int k = 0; k < ai_cars[i].Rays.Length; k++)
                        if (CollisionDetection.AABB_RAY(traffic[j].AABB, ai_cars[i].Rays[k]))
                            ai_cars[i].Sensor[k] = 1;
                }
            }
            //Update road
            road.Update(ai_cars[focused_ai]);
        }

        public void Draw(RenderTarget trgt)
        {
            road.Draw(trgt);

            foreach (AI_Car car in ai_cars)
            {
                Globals.SPRITE_CAR.TextureRect = car.Dead ? Globals.AIDEAD_TEXCOORDS : Globals.AI_TEXCOORDS;
                Globals.SPRITE_CAR.Position = car.Position;
                Globals.SPRITE_CAR.Rotation = car.Rotation;
                trgt.Draw(Globals.SPRITE_CAR);

                for(int i = 0; i < car.Rays.Length; i++)
                {
                    Color color = car.Sensor[i] == 1 ? Color.Black : Color.Green;
                    Vertex v1 = new Vertex(car.Rays[i].p1, color);
                    Vertex v2 = new Vertex(car.Rays[i].p2, color);
                    trgt.Draw(new Vertex[] {v1, v2}, PrimitiveType.Lines);
                }
            }

            foreach (Traffic_Car car in traffic)
            {
                switch (car.Type)
                {
                    case Traffic_Car.CarType.Traffic1: Globals.SPRITE_CAR.TextureRect = Globals.TRAFFIC1_TEXCOORDS; break;
                    case Traffic_Car.CarType.Traffic2: Globals.SPRITE_CAR.TextureRect = Globals.TRAFFIC2_TEXCOORDS; break;
                    case Traffic_Car.CarType.Traffic3: Globals.SPRITE_CAR.TextureRect = Globals.TRAFFIC3_TEXCOORDS; break;
                    case Traffic_Car.CarType.Traffic4: Globals.SPRITE_CAR.TextureRect = Globals.TRAFFIC4_TEXCOORDS; break;
                }
                Globals.SPRITE_CAR.Position = car.Position;
                Globals.SPRITE_CAR.Rotation = 0;
                trgt.Draw(Globals.SPRITE_CAR);
            }
        }

        public void Creat_AI_Car(Vector2f position)
        {
            ai_cars.Add(new AI_Car(position));
        }

        public void GenerateTraffic(int nbr)
        {
            //Clamp number
            nbr = Math.Clamp(nbr, 1, Globals.MAX_TRAFFIC - traffic.Count);

            //Contains all possible position for spawning a traffic car
            List<Vector2f> carPositions = new List<Vector2f>()
            {
                //First lane
                new Vector2f(-200,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*1),
                new Vector2f(-200,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*3),
                new Vector2f(-200,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*5),
                new Vector2f(-200,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*7),

                //Second lane
                new Vector2f(0000,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*1),
                new Vector2f(0000,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*3),
                new Vector2f(0000,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*5),
                new Vector2f(0000,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*7),

                //Third lane
                new Vector2f(0200,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*1),
                new Vector2f(0200,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*3),
                new Vector2f(0200,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*5),
                new Vector2f(0200,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*7)
            };

            //Generate traffic
            for (int i = 0; i < nbr; i++)
            {
                int typeIndex = GameMath.Rnd.Next(0, 4);
                int posIndex = GameMath.Rnd.Next(0, carPositions.Count);
                traffic.Add(new Traffic_Car(carPositions[posIndex], (Traffic_Car.CarType)typeIndex));
                carPositions.Remove(carPositions[posIndex]);
            }
        }
    }
}
