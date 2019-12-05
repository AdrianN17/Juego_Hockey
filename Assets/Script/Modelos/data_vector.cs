using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_vector
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public data_vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
