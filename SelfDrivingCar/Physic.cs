using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfDrivingCar
{

    struct AABB
    {
        public Vector2f p1;
        public Vector2f p2;
        public Vector2f p3;
        public Vector2f p4;
    }

    struct Ray
    {
        public Vector2f p1;
        public Vector2f p2;
    }
}
