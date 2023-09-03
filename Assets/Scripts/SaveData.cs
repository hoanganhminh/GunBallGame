using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.U2D.Animation;

namespace Assets.Scripts
{
    [Serializable]
    public class SaveData // Do not inherit from MonoBehaviour here
    {
        public List<MoverData> Characters { get; set; }
    }
}
