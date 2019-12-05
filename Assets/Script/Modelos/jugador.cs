

using Assets.Script.Modelos;
using Assets.Script.Modelos_ESharkNet;
using UnityEngine;

public class jugador:Convert_vector
{
    public GameObject disco;
    public bool usado;
    public int index_peer;
    public Vector3 mouse_pos;
    public Rigidbody rb;
    public Collider col;
    public jugador(GameObject disco)
    {
        this.index_peer = -1;
        this.disco = disco;
        this.usado = false;
        this.rb = disco.GetComponent<Rigidbody>();
        this.col = disco.GetComponent<Collider>();
    }

    public void add_player(int index_peer)
    {
        this.index_peer = index_peer;

        this.usado = true;
    }

    public void remove_player()
    {
        this.index_peer = -1;
        this.usado = false;
    }

    public void set_mouse_pos(Vector3 mouse_pos)
    {
        this.mouse_pos = mouse_pos;
    }

    public data_personaje get_data_para_enviar()
    {
        return new data_personaje(vec_to_obj(disco.transform.position), index_peer);
    }


    
}
