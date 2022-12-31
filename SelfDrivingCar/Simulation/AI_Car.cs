using SelfDrivingCar.NeuralNet;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{
    internal class AI_Car
    {
        //Properties
        bool dead = false;
        CarBrain brain = new CarBrain();
        float totalSeconds = 0;
        //Controls
        bool forwards = false;
        bool backwards = false;
        bool left = false;
        bool right = false;

        //Physic properties
        float rotation = 0;
        float speed = 0;
        Vector2f position;
        AABB aabb;
        Ray[] rays = new Ray[Globals.RAY_NBR];
        int[] sensor = new int[Globals.RAY_NBR];

        //Getters-Setters
        public bool Dead { get => dead; set => dead = value; }
        public bool Forwards { get => forwards; set => forwards = value; }
        public bool Backwards { get => backwards; set => backwards = value; }
        public bool Left { get => left; set => left = value; }
        public bool Right { get => right; set => right = value; }
        public float Rotation { get => rotation; }
        public float Speed { get => speed; }
        public Vector2f Position { get => position; }
        public AABB AABB { get => aabb; }
        public Ray[] Rays { get => rays; }
        public int[] Sensor { get => sensor; set => sensor = value; }
        public CarBrain Brain { get => brain; }
        public float TotalSeconds { get => totalSeconds; set => totalSeconds = value; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"> Car position </param>
        /// <param name="brain"> Car brain </param>
        public AI_Car(Vector2f position, float[,] w1, float[] b1, float[,] w2, float[] b2, bool mutate)
        {
            this.position = position;
            brain = new CarBrain();
            Array.Copy(w1, brain.Layer1.Weights, w1.Length);
            Array.Copy(b1, brain.Layer1.Biases, b1.Length);
            Array.Copy(w2, brain.Layer2.Weights, w2.Length);
            Array.Copy(b2, brain.Layer2.Biases, b2.Length);
            if (mutate) { brain.Mutate(); }
            Update();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"> Car position </param>
        public AI_Car(Vector2f position)
        {
            this.position = position;
            brain = new CarBrain();
            brain.Randomize();
            Update();
        }

        public void Update()
        {
            brain.ProcessInput(new float[] { sensor[0], sensor[1], sensor[2], sensor[3], sensor[4], sensor[5] });

            forwards = brain.Forward;
            backwards = brain.Backward;
            left = brain.Left;
            right = brain.Right;

            //Handle controls
            if (forwards)
            {
                speed += Globals.ACCELERATION * GameTime.DeltaTimeU;
            }
            if (backwards)
            {
                speed -= Globals.ACCELERATION * GameTime.DeltaTimeU;
            }
            if (left)
            {
                rotation -= Globals.TURN_RATE * GameTime.DeltaTimeU;
            }
            if (right)
            {
                rotation += Globals.TURN_RATE * GameTime.DeltaTimeU;
            }

            //Clamp car speed
            speed = Math.Clamp(speed, Globals.MAX_SPEED_CAR_REVERSE, Globals.MAX_SPEED_CAR);

            //Apply velocity to position
            position += GameMath.GetUnitVectorFromAngle(GameMath.ToRadian(rotation) - GameMath.ToRadian(90)) * speed * GameTime.DeltaTimeU;

            //Apply friction
            if (speed > 0) speed -= Globals.FRICTION * GameTime.DeltaTimeU;
            if (speed < 0) speed += Globals.FRICTION * GameTime.DeltaTimeU;

            //Update Axis-Align Bounding Box
            aabb.p1 = position + new Vector2f(-Globals.CAR_WIDTH / 3, -Globals.CAR_HEIGHT / 3);
            aabb.p2 = position + new Vector2f(Globals.CAR_WIDTH / 3, -Globals.CAR_HEIGHT / 3);
            aabb.p3 = position + new Vector2f(Globals.CAR_WIDTH / 3, Globals.CAR_HEIGHT / 3);
            aabb.p4 = position + new Vector2f(-Globals.CAR_WIDTH / 3, Globals.CAR_HEIGHT / 3);

            //Update Rays
            float deltaAngle = GameMath.ToRadian(Globals.RAY_FIELD / (rays.Length - 1));
            float currentAngle = GameMath.ToRadian(rotation - Globals.RAY_FIELD / 2);
            Vector2f startPoint = position + GameMath.GetUnitVectorFromAngle(GameMath.ToRadian(rotation - 90)) * (Globals.CAR_HEIGHT / 2);
            for (int i = 0; i < rays.Length; i++)
            {
                rays[i].p1 = startPoint;
                rays[i].p2 = GameMath.GetUnitVectorFromAngle(currentAngle) * Globals.RAY_LENGTH + startPoint;
                currentAngle -= deltaAngle;
            }
        }

        public void ResetSensor()
        {
            for (int i = 0; i < sensor.Length; i++)
            {
                sensor[i] = 0;
            }
        }
    }
}
