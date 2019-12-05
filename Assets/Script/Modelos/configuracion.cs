using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    public class configuracion
    {
        public string ip { get; set; }
        public ushort port { get; set; }

        public bool automatico { get; set; }

        public configuracion(string ip, ushort port, bool automatico)
        {
            this.ip = ip;
            this.port = port;
            this.automatico = automatico;
        }
    }
}
