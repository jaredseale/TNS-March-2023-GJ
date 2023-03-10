using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Dialogue dialogue;
    [SerializeField] PlayableDirector playDirector;
    private DialogueManager dialogueManager;
    private bool isPlayingCutsceneDialogue;
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void PauseCutesceneForDialogue()
    {
        playDirector.Pause();
        dialogueManager.StartDialogue(dialogue);
        isPlayingCutsceneDialogue = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.animator.GetBool("isOpen") == false && isPlayingCutsceneDialogue == true)
        {
            playDirector.Play();
        }
    }
}
