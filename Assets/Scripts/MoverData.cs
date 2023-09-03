using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class MoverData
    {
        public string Type;
        public int Power;
        public Vector3 Position;
        public Vector3 Destination;

        public override string ToString()
        {
            return Position+" "+Destination;
        }
    }
}
