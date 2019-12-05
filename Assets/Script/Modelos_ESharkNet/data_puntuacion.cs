using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos_ESharkNet
{
    [Serializable]
    public class data_puntuacion
    {
        public int p1 { get; set; }
        public int p2 { get; set; }
        public data_puntuacion(int p1,int p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }
    }
}
