using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class PostProcessingScript : MonoBehaviour
{

    public PostProcessVolume volume;
    private const int MAXDISTANCE = 10;

    private Vignette _vignette;

    private Bloom _bloom;

    public Transform players;

    public int currentPlayer = 0;
    public float maxDamageWaitTime;
    private float damageWaitTime;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out _vignette);
        volume.profile.TryGetSettings(out _bloom);

        _vignette.intensity.value = 0;
        _bloom.intensity.value = 0;
        damageWaitTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        GameObject[] rhinos = GameObject.FindGameObjectsWithTag("rhino");
        float dist = float.MaxValue;
        // Active player
        foreach(GameObject rhino in rhinos)
        {
            dist = Mathf.Min(Mathf.Sqrt(Mathf.Pow(rhino.transform.position.x - players.GetChild(currentPlayer).transform.position.x, 2) + Mathf.Pow(rhino.transform.position.y - players.GetChild(currentPlayer).transform.position.y, 2)), dist);
        }
        if (dist > MAXDISTANCE)
        {
            if(dist >= MAXDISTANCE + 3)
            {
                _bloom.intensity.value = Mathf.PingPong(Time.time, 1) * 20;
                damageWaitTime = System.Math.Min(damageWaitTime + Time.deltaTime, maxDamageWaitTime);
                if (damageWaitTime == maxDamageWaitTime)
                {
                    GameObject player = players.GetChild(currentPlayer).gameObject;
                    player.GetComponent<Player>().Knock(player.GetComponent<Rigidbody2D>(), 0, 1);
                    damageWaitTime = 0;
                }
            }
            else
            {
                _bloom.intensity.value = 0;
            }
            _vignette.intensity.value = Mathf.Min((float)(dist - MAXDISTANCE) / 10, 0.5f);
        }
        else
        {
            _bloom.intensity.value = 0;
            _vignette.intensity.value = 0;
        }
    }
}
