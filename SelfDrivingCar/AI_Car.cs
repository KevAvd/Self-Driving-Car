using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SelfDrivingCar
{
    internal class AI_Car
    {
        //Properties
        bool dead = false;

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


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"> Car position </param>
        public AI_Car(Vector2f position)
        {
            this.position = position;
            Update();
        }

        public void Update()
        {
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
            position += (GameMath.GetUnitVectorFromAngle(GameMath.ToRadian(rotation) - GameMath.ToRadian(90)) * speed) * GameTime.DeltaTimeU;

            //Apply friction
            if (speed > 0) speed -= Globals.FRICTION * GameTime.DeltaTimeU;
            if (speed < 0) speed += Globals.FRICTION * GameTime.DeltaTimeU;

            //Update Axis-Align Bounding Box
            aabb.p1 = position + new Vector2f(-Globals.CAR_WIDTH / 3, -Globals.CAR_HEIGHT / 3);
            aabb.p2 = position + new Vector2f( Globals.CAR_WIDTH / 3, -Globals.CAR_HEIGHT / 3);
            aabb.p3 = position + new Vector2f( Globals.CAR_WIDTH / 3,  Globals.CAR_HEIGHT / 3);
            aabb.p4 = position + new Vector2f(-Globals.CAR_WIDTH / 3,  Globals.CAR_HEIGHT / 3);

            //Update Rays
            float deltaAngle = GameMath.ToRadian(Globals.RAY_FIELD / (float)(rays.Length-1));
            float currentAngle = GameMath.ToRadian(rotation- Globals.RAY_FIELD/2);
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
            for(int i = 0; i < sensor.Length; i++)
            {
                sensor[i] = 0;
            }
        }
    }
}
