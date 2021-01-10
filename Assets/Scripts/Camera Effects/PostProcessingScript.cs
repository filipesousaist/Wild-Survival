using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessingScript : MonoBehaviour
{

    public PostProcessVolume volume;
    private const int MAXDISTANCE = 10;

    //private Vignette _vignette;

    //private Bloom _bloom;

    public Transform players;

    public Light2D[] lights;

    public Light2D redLight;

    public int currentPlayer = 0;
    public float maxDamageWaitTime;
    private float damageWaitTime;

    private float r = 0;

    private float g = 0;

    private float b = 1;

    private Color baseColor;

    private Color color = Color.red;


    // Start is called before the first frame update
    void Start()
    {
       /* volume.profile.TryGetSettings(out _vignette);
        volume.profile.TryGetSettings(out _bloom);*/

        lights = FindObjectsOfType<Light2D>();

        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].name.Equals("RedLight"))
            {
                redLight = lights[i];
            }
        }
        redLight.intensity = 0;

        baseColor = players.GetChild(currentPlayer).gameObject.GetComponent<SpriteRenderer>().color;

        /*_vignette.intensity.value = 0;
        _bloom.intensity.value = 0;*/
        damageWaitTime = 0;
    }
    
    // FixedUpdate is called periodically, with a fixed period
    void FixedUpdate()
    {
        GameObject[] rhinos = GameObject.FindGameObjectsWithTag("rhino");
        float dist = float.MaxValue;
        // Active player
        foreach (GameObject rhino in rhinos)
        {
            dist = Mathf.Min(Mathf.Sqrt(Mathf.Pow(rhino.transform.position.x - players.GetChild(currentPlayer).transform.position.x, 2) + Mathf.Pow(rhino.transform.position.y - players.GetChild(currentPlayer).transform.position.y, 2)), dist);
        }
        if (dist > MAXDISTANCE)
        {
            /*if(dist >= MAXDISTANCE + 3)
            {*/
                //_bloom.intensity.value = Mathf.PingPong(Time.time, 1) * 20;
                redLight.intensity = Mathf.PingPong(Time.time, 0.8f) / 4 ;
            g = Mathf.PingPong(Time.time, 1f);
            b = g;
            color.g = g;
            color.b = b;
            players.GetChild(currentPlayer).gameObject.GetComponent<SpriteRenderer>().color = color;
                damageWaitTime = Mathf.Min(damageWaitTime + Time.deltaTime, maxDamageWaitTime);
                if (damageWaitTime == maxDamageWaitTime)
                {
                    GameObject player = players.GetChild(currentPlayer).gameObject;
                    player.GetComponent<Player>().TakeRadiationDamage(1);
                    damageWaitTime = 0;
                }
           /* }
            else
            {
                //_bloom.intensity.value = 0;
                redLight.intensity = 0;
            }
            _vignette.intensity.value = Mathf.Min((float)(dist - MAXDISTANCE) / 10, 0.5f);*/
        }
        else
        {
            /*_bloom.intensity.value = 0;
            _vignette.intensity.value = 0;*/
            redLight.intensity = 0;
            players.GetChild(currentPlayer).gameObject.GetComponent<SpriteRenderer>().color = baseColor;
        }
    }

    /*  Faz com que todos os ativistas fora do alcance do rhinos percam vida - bugado quando morrem todos ao mesmo tempo
    void FixedUpdate()
    {
        GameObject[] rhinos = GameObject.FindGameObjectsWithTag("rhino");
        PlayerMovement[] playersList = players.GetComponent<ActivistsManager>().players;
        float dist = float.MaxValue;
        foreach (PlayerMovement player in playersList)
        { 
            foreach (GameObject rhino in rhinos)
            {
                dist = Mathf.Min(Mathf.Sqrt(Mathf.Pow(rhino.transform.position.x - player.transform.position.x, 2) + Mathf.Pow(rhino.transform.position.y - player.transform.position.y, 2)), dist);
            }
            if (dist > MAXDISTANCE)
            {
                if(dist >= MAXDISTANCE + 3)
                {
                    if(player.transform == players.GetChild(currentPlayer).transform){
                        _bloom.intensity.value = Mathf.PingPong(Time.time, 1) * 20;
                     }
                    damageWaitTime = Mathf.Min(damageWaitTime + Time.deltaTime, maxDamageWaitTime);
                    if (damageWaitTime == maxDamageWaitTime)
                    {
                        player.gameObject.GetComponent<Player>().Knock(player.GetComponent<Rigidbody2D>(), 0, 1);
                        damageWaitTime = 0;
                    }
                }
                else
                {   
                    if(player.transform == players.GetChild(currentPlayer).transform){
                        _bloom.intensity.value = 0;
                     }
                }
                if(player.transform == players.GetChild(currentPlayer).transform){
                        _vignette.intensity.value = Mathf.Min((float)(dist - MAXDISTANCE) / 10, 0.5f);
                }
            }
            else
            {
                if(player.transform == players.GetChild(currentPlayer).transform){
                    _bloom.intensity.value = 0;
                    _vignette.intensity.value = 0;
                }
            }
        }
    }
     */
}
