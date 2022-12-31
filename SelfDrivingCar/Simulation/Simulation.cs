using SelfDrivingCar.NeuralNet;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar.Simulation
{
    internal class Simulation
    {
        List<Traffic_Car> traffic = new List<Traffic_Car>();
        List<AI_Car> ai_cars = new List<AI_Car>();
        Population population = new Population();
        Road road;
        int focused_ai = 0;
        bool ALL_AI_DEAD = false;
        int generation = 0;
        int nbrAI = Globals.AI_PER_GENERATION;

        public AI_Car Focused_AI { get => ai_cars[focused_ai]; }
        public int Generation { get => generation; set => generation = value; }
        public int NbrAI { get => nbrAI; set => nbrAI = value; }

        public Simulation()
        {
            Globals.SPRITE_CAR.Origin = new Vector2f(12 / 2, 16 / 2);
            Globals.SPRITE_CAR.Scale = new Vector2f(Globals.CAR_WIDTH / 12, Globals.CAR_HEIGHT / 16);
            ai_cars.Clear();
            traffic.Clear();
            road = new Road();
            GenerateTraffic(8);
            GenerateDeadTraffic(4);
            for (int i = 0; i < Globals.AI_PER_GENERATION; i++)
            {
                ai_cars.Add(new AI_Car(new Vector2f(0, 0)));
            }
        }

        public void Update()
        {
            for (int i = traffic.Count - 1; i >= 0; i--)
            {
                //Update traffic
                traffic[i].Update();

                //Remove traffic when outside of road range
                if (traffic[i].Position.Y > road.BackRoad.Y + Globals.ROAD_HEIGHT / 2 || traffic[i].Position.Y < road.FrontRoad.Y)
                {
                    if (traffic[i].Type == Traffic_Car.CarType.DeadTraffic)
                    {
                        GenerateDeadTraffic(1);
                    }
                    else
                    {
                        GenerateTraffic(1);
                    }

                    traffic.Remove(traffic[i]);
                }
            }

            float bestDistance = 0;
            ALL_AI_DEAD = true;
            for (int i = 0; i < ai_cars.Count; i++)
            {
                if (ai_cars[i].Dead) continue;
                ALL_AI_DEAD = false;

                //Switch the focused ai according to the distance traveled
                if (ai_cars[i].Position.Y < bestDistance)
                {
                    bestDistance = ai_cars[i].Position.Y;
                    focused_ai = i;
                }

                //Get neural net outputs
                float[] outputs = population.ProcessInputWithBrain(i,new float[] 
                {
                    ai_cars[i].Sensor[0].value,
                    ai_cars[i].Sensor[1].value,
                    ai_cars[i].Sensor[2].value,
                    ai_cars[i].Sensor[3].value,
                    ai_cars[i].Sensor[4].value,
                    ai_cars[i].Sensor[5].value,
                    ai_cars[i].Rotation,
                    ai_cars[i].Speed
                });

                //Move car according to the neural net outputs
                if (outputs[0] == 1)
                    ai_cars[i].Forwards = true;
                else
                    ai_cars[i].Forwards = false;

                if (outputs[1] == 1)
                    ai_cars[i].Backwards = true;
                else
                    ai_cars[i].Backwards = false;

                if (outputs[2] == 1)
                    ai_cars[i].Left = true;
                else
                    ai_cars[i].Left = false;

                if (outputs[3] == 1)
                    ai_cars[i].Right = true;
                else
                    ai_cars[i].Right = false;

                //Update car
                ai_cars[i].Update();

                //Kill car if it leave road
                if (ai_cars[i].Position.X > 300 || ai_cars[i].Position.X < -300 || ai_cars[i].Position.Y > road.BackRoad.Y + Globals.ROAD_HEIGHT)
                {
                    ai_cars[i].Dead = true;
                    nbrAI--;
                    population.SetBrainDistance(i, ai_cars[i].Position.Y * -1);
                    continue;
                }

                //Reset sensors
                ai_cars[i].ResetSensor();

                //Sensors check road limit
                for (int j = 0; j < ai_cars[i].Sensor.Length; j++)
                {
                    if (CollisionDetection.AABB_SENSOR(road.RightAABB, ai_cars[i].Sensor[j], out Vector2f pNear1))
                    {
                        ai_cars[i].Sensor[j] = UpdateSensor(ai_cars[i].Sensor[j], pNear1);
                    }
                    else if (CollisionDetection.AABB_SENSOR(road.LeftAABB, ai_cars[i].Sensor[j], out Vector2f pNear2))
                    {
                        ai_cars[i].Sensor[j] = UpdateSensor(ai_cars[i].Sensor[j], pNear2);
                    }
                }

                //Handle collisions
                for (int j = 0; j < traffic.Count; j++)
                {
                    if (CollisionDetection.AABB_AABB(ai_cars[i].AABB, traffic[j].AABB))
                    {
                        ai_cars[i].Dead = true;
                        nbrAI--;
                        population.SetBrainDistance(i, ai_cars[i].Position.Y * -1);
                        break;
                    }
                    for (int k = 0; k < ai_cars[i].Sensor.Length; k++)
                    {
                        if (CollisionDetection.AABB_SENSOR(traffic[j].AABB, ai_cars[i].Sensor[k], out Vector2f pNear))
                        {
                            ai_cars[i].Sensor[k] = UpdateSensor(ai_cars[i].Sensor[k], pNear);
                        }
                    }
                }
            }

            //Update road
            road.Update(ai_cars[focused_ai]);

            if(ALL_AI_DEAD || Inputs.IsClicked(Keyboard.Key.N))
            {
                population.NextGeneration();
                generation++;
                nbrAI = Globals.AI_PER_GENERATION;
                ai_cars.Clear();
                traffic.Clear();
                road = new Road();
                GenerateTraffic(8);
                GenerateDeadTraffic(4);
                for(int i = 0; i < Globals.AI_PER_GENERATION; i++)
                {
                    ai_cars.Add(new AI_Car(new Vector2f(0, 0)));
                }
            }
        }

        Sensor UpdateSensor(Sensor sensor, Vector2f pNear)
        {
            sensor.hitPoint = pNear;
            sensor.hitted = true;
            float rayLenght = GameMath.GetVectorLength(sensor.p2 - sensor.p1);
            float hitLenght = GameMath.GetVectorLength(sensor.hitPoint - sensor.p1);
            sensor.value = (1-(hitLenght / rayLenght)) * 100;
            return sensor;
        }

        public void Draw(RenderTarget trgt)
        {
            road.Draw(trgt);

            foreach (Traffic_Car car in traffic)
            {
                Globals.SPRITE_CAR.Color = new Color(255, 255, 255, 255);
                switch (car.Type)
                {
                    case Traffic_Car.CarType.Traffic1: Globals.SPRITE_CAR.TextureRect = Globals.TRAFFIC1_TEXCOORDS; break;
                    case Traffic_Car.CarType.Traffic2: Globals.SPRITE_CAR.TextureRect = Globals.TRAFFIC2_TEXCOORDS; break;
                    case Traffic_Car.CarType.Traffic3: Globals.SPRITE_CAR.TextureRect = Globals.TRAFFIC3_TEXCOORDS; break;
                    case Traffic_Car.CarType.Traffic4: Globals.SPRITE_CAR.TextureRect = Globals.TRAFFIC4_TEXCOORDS; break;
                    case Traffic_Car.CarType.DeadTraffic: Globals.SPRITE_CAR.TextureRect = Globals.AIDEAD_TEXCOORDS; break;
                }
                Globals.SPRITE_CAR.Position = car.Position;
                Globals.SPRITE_CAR.Rotation = 0;
                trgt.Draw(Globals.SPRITE_CAR);
            }

            for(int j = 0; j < ai_cars.Count; j++)
            {
                AI_Car car = ai_cars[j];
                if (car.Dead) { continue; }
                Globals.SPRITE_CAR.Color = new Color(255, 255, 255, 100);
                Globals.SPRITE_CAR.TextureRect = Globals.AI_TEXCOORDS;
                Globals.SPRITE_CAR.Position = car.Position;
                Globals.SPRITE_CAR.Rotation = car.Rotation;
                trgt.Draw(Globals.SPRITE_CAR);
                if(j != focused_ai) { continue; }
                for (int i = 0; i < car.Sensor.Length; i++)
                {
                    if (car.Sensor[i].hitted)
                    {
                        Color color = Color.Green;
                        Vertex v1 = new Vertex(car.Sensor[i].p1, color);
                        Vertex v2 = new Vertex(car.Sensor[i].hitPoint, color);
                        trgt.Draw(new Vertex[] { v1, v2 }, PrimitiveType.Lines);
                        color = Color.Black;
                        v1 = new Vertex(car.Sensor[i].hitPoint, color);
                        v2 = new Vertex(car.Sensor[i].p2, color);
                        trgt.Draw(new Vertex[] { v1, v2 }, PrimitiveType.Lines);
                    }
                    else
                    {
                        Color color = Color.Green;
                        Vertex v1 = new Vertex(car.Sensor[i].p1, color);
                        Vertex v2 = new Vertex(car.Sensor[i].p2, color);
                        trgt.Draw(new Vertex[] { v1, v2 }, PrimitiveType.Lines);
                    }
                }
            }
        }

        void GenerateDeadTraffic(int nbr)
        {
            //Clamp number
            nbr = Math.Clamp(nbr, 1, Globals.MAX_DEAD_TRAFFIC);

            //Contains all possible position for spawning a traffic car
            List<Vector2f> carPositions = new List<Vector2f>()
            {
                //Left Side
                new Vector2f(-300,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*1),
                new Vector2f(-300,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*3),
                new Vector2f(-300,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*5),
                new Vector2f(-300,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*7),

                //Right side
                new Vector2f(0300,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*1),
                new Vector2f(0300,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*3),
                new Vector2f(0300,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*5),
                new Vector2f(0300,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*7),

                //Middle-left
                new Vector2f(-100,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*1),
                new Vector2f(-100,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*3),
                new Vector2f(-100,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*5),
                new Vector2f(-100,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*7),

                //Middle-right
                new Vector2f(0100,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*1),
                new Vector2f(0100,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*3),
                new Vector2f(0100,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*5),
                new Vector2f(0100,road.FrontRoad.Y+(Globals.CAR_HEIGHT+20)*7)
            };

            //Generate traffic
            for (int i = 0; i < nbr; i++)
            {
                int posIndex = GameMath.Rnd.Next(0, carPositions.Count);
                traffic.Add(new Traffic_Car(carPositions[posIndex], Traffic_Car.CarType.DeadTraffic));
                carPositions.Remove(carPositions[posIndex]);
            }
        }

        void GenerateTraffic(int nbr)
        {
            //Clamp number
            nbr = Math.Clamp(nbr, 1, Globals.MAX_TRAFFIC);

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
