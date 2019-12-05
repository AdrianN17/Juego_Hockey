using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
using Assets.Script.Modelos;
using Assets.Script.Modelos_ESharkNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class servidor : Convert_vector
{
    public Server server;
    
    private List<jugador> lista_jugadores;

    public GameObject jugador_1;
    public GameObject jugador_2;

    public GameObject disco;

    public string ip;
    public ushort port;
    public int clients;
    public int channels;
    public int timeout;

    private Vector3 disco_pos;

    public List<int> puntuaciones;

    public float velocidad;

    public Timer timer;




    void Start()
    {
        verificar_file_json();

        puntuaciones = new List<int>();
        puntuaciones.Add(0);
        puntuaciones.Add(0);

        lista_jugadores = new List<jugador>();

        lista_jugadores.Add(new jugador(jugador_1));
        lista_jugadores.Add(new jugador(jugador_2));

        disco_pos = disco.transform.position;



        server = new Server(new LocalIP().SetLocalIP(),port,clients,channels,timeout);

        server.AddTrigger("Connect", delegate (ENet.Event net_event)
        {
            int index_peer = server.AddPeer(net_event);

            lista_jugadores[index_peer].add_player(index_peer);

            Debug.Log("Usuario nuevo : " + index_peer);

            server.Send("inicializar_id", new data_id(index_peer), net_event.Peer);

            if(server.GetListClientsCount()==2)
            {
                server.SendToAll("iniciar_juego", null);
            }
        });

        server.AddTrigger("dar_posicion_mouse", delegate (ENet.Event net_event)
        {
            var data = server.Decode(net_event.Packet);
            var obj = (data_mouse_pos)data.value;
    

            lista_jugadores[obj.id].mouse_pos = obj_to_vec(obj.vec);
        });

        server.AddTrigger("Disconnect", delegate (ENet.Event net_event)
        {
            int i = server.RemovePeer(net_event);

            server.SendToAll("falta_usuarios", null); // new data_id(i)

            puntuaciones[0] = 0;
            puntuaciones[1] = 0;
        });


        timer = Timer.Register(0.1f, () => enviar_posiciones_todos(), isLooped: true);

    }

    void verificar_file_json()
    {
        if (!File.Exists(Application.persistentDataPath + "/conf_server.json"))
        {
            var conf = new configuracion(new LocalIP().SetLocalIP(), 22122, true);


            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/conf_server.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, conf);
            }

            read_file_json();
        }
        else
        {
            read_file_json();
        }
    }

    void read_file_json()
    {
        var text = File.ReadAllText(Application.persistentDataPath + "/conf_server.json");

        var conf = JsonConvert.DeserializeObject<configuracion>(text);

        if (!conf.automatico)
        {
            ip = conf.ip;
            port = conf.port;
        }
    }

    public void guardar_puntuacion(int pos)
    {
        puntuaciones[pos] ++;

        if(puntuaciones[pos] == 7)
        {
            puntuaciones[0] = 0;
            puntuaciones[1] = 0;
        }

        server.SendToAll("dar_puntuacion", new data_puntuacion(puntuaciones[0],puntuaciones[1]));

        //Debug.LogError("Score : " + puntuaciones[0] + " , " + puntuaciones[1]); 
    }
    
    void Update()
    {

        if(disco.transform.position.y<0)
        {
            disco.transform.position = disco_pos;
        }


        server.update();
    }

    private void FixedUpdate()
    {
        if (server.GetListClientsCount() == 2)
        {

            foreach (var jugador in lista_jugadores)
            {
                if(jugador.usado)
                {
                    mover(jugador.disco, jugador.col.bounds.center, jugador.mouse_pos, jugador.rb);
                }
            }
        }
    }

    public void enviar_posiciones_todos()
    {
        if (server.GetListClientsCount() == 2)
        {
            server.SendToAll("enviar_posiciones_todos", new data_juego(lista_jugadores[0].get_data_para_enviar(), lista_jugadores[1].get_data_para_enviar(), vec_to_obj(disco.transform.position)));
        } 
    }

    public void mover(GameObject mi_jugador,Vector3 personaje_pos,Vector3 mouse_pos,Rigidbody rb)
    {
        if (mi_jugador != null)
        {
           
            var distancia = FlatDistance(personaje_pos, mouse_pos);

            if (distancia > 0.5)
            {
                var dt = Time.deltaTime;
                var vec = mi_jugador.GetComponent<Collider>().bounds.center;

                Vector3 direction = (mouse_pos - vec).normalized;
                Quaternion rotate = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));



                mi_jugador.transform.rotation = Quaternion.Slerp(mi_jugador.transform.rotation, rotate, dt * 10f);
                rb.rotation = mi_jugador.transform.rotation;

                rb.AddForce(mi_jugador.transform.forward * velocidad * rb.mass * dt);
            }
            
        }
    }

    public static float FlatDistance(Vector3 pos1, Vector3 pos2)
    {
        pos1.y = pos1.z;
        pos2.y = pos2.z;
        return Vector2.Distance(pos1, pos2);
    }

    private void OnDestroy()
    {
        server.Destroy();
    }

}
