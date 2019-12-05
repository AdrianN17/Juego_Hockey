using Assets.Script.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos_ESharkNet
{
    [Serializable]
    public class data_personaje
    {
        public data_vector vec { get; set; }
        public int id { get; set; }

        public data_personaje(data_vector vec, int id)
        {
            this.vec = vec;
            this.id = id;
        }
    }
}
