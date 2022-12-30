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
        Ray ray;

        //Getters-Setters
        public bool Dead { get => dead; set => dead = value; }
        public bool Forwards { get => forwards; set => forwards = value; }
        public bool Backwards { get => backwards; set => backwards = value; }
        public bool Left { get => left; set => left = value; }
        public bool Right { get => right; set => right = value; }
        public float Rotation { get => rotation; }
        public float Speed { get => speed; }
        public Vector2f Position { get => position; }
        public AABB Aabb { get => aabb; }
        public Ray Ray { get => ray; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"> Car position </param>
        public AI_Car(Vector2f position)
        {
            this.position = position;
        }

        public void Update()
        {
            //Don't update car if dead
            if (dead) return;

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
            aabb.p1 = position + new Vector2f(-Globals.CAR_WIDTH / 2, -Globals.CAR_HEIGHT / 2);
            aabb.p2 = position + new Vector2f( Globals.CAR_WIDTH / 2, -Globals.CAR_HEIGHT / 2);
            aabb.p3 = position + new Vector2f( Globals.CAR_WIDTH / 2,  Globals.CAR_HEIGHT / 2);
            aabb.p4 = position + new Vector2f(-Globals.CAR_WIDTH / 2,  Globals.CAR_HEIGHT / 2);
        }
    }
}
