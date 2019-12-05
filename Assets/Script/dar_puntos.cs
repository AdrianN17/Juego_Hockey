using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dar_puntos : MonoBehaviour
{
    // Start is called before the first frame update
    public servidor server;
    public int id_parte;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Disco"))
        {
            server.guardar_puntuacion(id_parte);

        }
    }
}
