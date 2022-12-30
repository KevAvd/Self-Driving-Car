using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{
    internal class Traffic_Car
    {
        //Physic properties
        Vector2f position;
        AABB aabb;
        CarType type;

        public Vector2f Position { get => position; }
        public AABB AABB { get => aabb; }
        internal CarType Type { get => type; set => type = value; }

        public enum CarType
        {
            Traffic1, Traffic2, Traffic3, Traffic4
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position"> Car position </param>
        public Traffic_Car(Vector2f position, CarType type)
        {
            this.position = position;
            this.type = type;
            Update();
        }

        public void Update()
        {
            //Update position
            position.Y -= Globals.MAX_SPEED_TRAFFIC * GameTime.DeltaTimeU;

            //Update Axis-Align Bounding Box
            aabb.p1 = position + new Vector2f(-Globals.CAR_WIDTH / 2, -Globals.CAR_HEIGHT / 2);
            aabb.p2 = position + new Vector2f( Globals.CAR_WIDTH / 2, -Globals.CAR_HEIGHT / 2);
            aabb.p3 = position + new Vector2f( Globals.CAR_WIDTH / 2,  Globals.CAR_HEIGHT / 2);
            aabb.p4 = position + new Vector2f(-Globals.CAR_WIDTH / 2,  Globals.CAR_HEIGHT / 2);
        }
    }
}
