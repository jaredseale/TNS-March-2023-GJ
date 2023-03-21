using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingMusicManager : MonoBehaviour
{
    public AudioSource fullTrack;
    public AudioSource loopTrack;

    void Update() {
        if (fullTrack.isPlaying == false) {
            fullTrack.gameObject.SetActive(false);
            loopTrack.gameObject.SetActive(true);
        }

    }
}
