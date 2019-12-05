using Assets.Libs.Esharknet;
using Assets.Libs.Esharknet.IP;
using Assets.Script.Modelos;
using Assets.Script.Modelos_ESharkNet;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class cliente : Convert_vector
{

    public Client client;

    public GameObject jugador_1;
    public GameObject jugador_2;

    public GameObject disco;

    public string ip;
    public ushort port;
    public int channels;
    public int timeout;

    public int mi_id;

    public Camera camera;
    private List<GameObject> lista_usuarios;

    public Text texto;
    public Text Text_estado;

    public Timer timer;

    private bool iniciar;


    // Start is called before the first frame update
    void Start()
    {
        verificar_file_json();



        texto.text = "Score : " + 0 + " | " + 0;
        Text_estado.text = "Esperando Jugadores";

        client = new Client(ip, port, channels, timeout);

        lista_usuarios = new List<GameObject>() { jugador_1, jugador_2 };


        mi_id = -1;

        jugador_1.GetComponent<Rigidbody>().freezeRotation = true;
        jugador_2.GetComponent<Rigidbody>().freezeRotation = true;

        

        client.AddTrigger("inicializar_id", delegate (ENet.Event net_event)
        {
            var data = client.Decode(net_event.Packet);
            var obj = (data_id)data.value;

            mi_id = obj.id;

            Debug.Log("mi id es : " + mi_id);
        });

        client.AddTrigger("iniciar_juego", delegate (ENet.Event net_event)
        {
            Text_estado.text = "En partida";
            iniciar = true;
        });



        client.AddTrigger("enviar_posiciones_todos", delegate (ENet.Event net_event)
        {
            var data = client.Decode(net_event.Packet);
            var obj = (data_juego)data.value;

            if(obj.vec_1.id != -1 && obj.vec_2.id != -1)
            {
                lista_usuarios[obj.vec_1.id].transform.position = obj_to_vec(obj.vec_1.vec);
                lista_usuarios[obj.vec_2.id].transform.position = obj_to_vec(obj.vec_2.vec);
                disco.transform.position = obj_to_vec(obj.vec_disco);
            }
                
        });

        client.AddTrigger("dar_puntuacion", delegate (ENet.Event net_event)
        {
            var data = client.Decode(net_event.Packet);
            var obj = (data_puntuacion)data.value;

            texto.text = "Score : " + obj.p1 + " | " + obj.p2;
        });

        client.AddTrigger("falta_usuarios", delegate (ENet.Event net_event)
        {
            Text_estado.text = "Esperando Jugadores";
            texto.text = "Score : " + 0 + " | " + 0;

            iniciar = false;
        });

        timer = Timer.Register(0.1f, () => capturar_mouse_pos(), isLooped: true);

        //timer = Timer.Register(0.1f, () => capturar_mouse_pos());

    }

    void verificar_file_json()
    {
        if (!File.Exists(Application.persistentDataPath + "/conf.json"))
        {
            var conf = new configuracion(new LocalIP().SetLocalIP(), 22122, true);


            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/conf.json"))
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
        var text = File.ReadAllText(Application.persistentDataPath + "/conf.json");

        var conf = JsonConvert.DeserializeObject<configuracion>(text);

        if (!conf.automatico)
        {
            ip = conf.ip;
            port = conf.port;
        }
    }

    void Update()
    {
        client.update();
    }

    public void capturar_mouse_pos()
    {
        if (mi_id != -1 && iniciar)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) //check if the ray hit something
            {
                var vec = vec_to_obj(hit.point);

                client.Send("dar_posicion_mouse", new data_mouse_pos(vec, mi_id));
            }
        }
    }

    private void OnDestroy()
    {
        Timer.CancelAllRegisteredTimers();
        client.Destroy();
    }


    public static float FlatDistance(Vector3 pos1, Vector3 pos2)
    {
        pos1.y = pos1.z;
        pos2.y = pos2.z;
        return Vector2.Distance(pos1, pos2);
    }

    

}
