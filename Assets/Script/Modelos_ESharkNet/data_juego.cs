using Assets.Script.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos_ESharkNet
{
    [Serializable]
    public class data_juego
    {
        public data_personaje vec_1 { get; set; }
        public data_personaje vec_2 { get; set; }
        public data_vector vec_disco { get; set; }

        public data_juego(data_personaje vec_1, data_personaje vec_2, data_vector vec_disco)
        {
            this.vec_1 = vec_1;
            this.vec_2 = vec_2;
            this.vec_disco = vec_disco;
        }
    }
}
