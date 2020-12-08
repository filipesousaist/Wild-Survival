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
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out _vignette);
        volume.profile.TryGetSettings(out _bloom);

        _vignette.intensity.value = 0;
        _bloom.intensity.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");
        GameObject[] rhinos = GameObject.FindGameObjectsWithTag("rhino");
        // Active player
        foreach(GameObject rhino in rhinos)
        {
            var dist = Mathf.Sqrt(Mathf.Pow(rhino.transform.position.x - players[0].transform.position.x, 2) + Mathf.Pow(rhino.transform.position.y - players[0].transform.position.y, 2));
            if (dist > MAXDISTANCE)
            {
                if(dist >= MAXDISTANCE + 3)
                {
                    _bloom.intensity.value = Mathf.PingPong(Time.time, 1) * 20;
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
            }
        }
    }
}
