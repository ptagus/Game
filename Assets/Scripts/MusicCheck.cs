using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = MusicSettings.volume;
        GetComponent<AudioSource>().mute = MusicSettings.music;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
