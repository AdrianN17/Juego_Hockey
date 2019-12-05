using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    public class Convert_vector : MonoBehaviour
    {
        public static data_vector vec_to_obj(Vector3 vec)
        {
            return new data_vector(vec.x, vec.y, vec.z);
        }

        public static Vector3 obj_to_vec(data_vector vec)
        {
            return new Vector3((float)vec.x, (float)vec.y, (float)vec.z);
        }
    }
}
