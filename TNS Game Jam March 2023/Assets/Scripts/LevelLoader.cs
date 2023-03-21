using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    public float loadSceneDelay = 2f;
    [SerializeField] float musicVolumeToFadeTo;

    public void Awake() {
        //mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
        //mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));

        GetComponent<Animator>().SetTrigger("wipeIn");
    }

    private void Start() {
        
    }

    public void LoadSceneWithDelay(string sceneName) {
        GetComponent<Animator>().SetTrigger("wipeOut");
        StartCoroutine("Delay", sceneName);
        StartCoroutine(FadeMixerGroup.StartFade(mixer, "MusicVolume", loadSceneDelay, musicVolumeToFadeTo));

    }

    IEnumerator Delay(string sceneName) {
        yield return new WaitForSeconds(loadSceneDelay);
        yield return new WaitForSeconds(0.1f); //this lets the music player destroy itself before the next scene is loaded

        SceneManager.LoadScene(sceneName);
    }
}