using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos_ESharkNet
{
    [Serializable]
    public class data_id
    {
        public int id { get; set; }
        public data_id(int id)
        {
            this.id = id;
        }
    }
}
