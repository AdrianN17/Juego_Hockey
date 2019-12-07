using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rebote : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Disco"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody>();

            var vec = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);

            rb.AddForce(vec*rb.mass*100, ForceMode.VelocityChange);

        }
    }
}
