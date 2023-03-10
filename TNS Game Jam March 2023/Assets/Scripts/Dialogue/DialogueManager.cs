using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    private Queue<string> sentences;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public float textSpeed;

    public Animator animator;
    private InputMaster playerControls;

    void Start()
    {
        sentences = new Queue<string>();
        playerControls = new InputMaster();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        playerControls.Player.Jump.Enable();
        nameText.text = dialogue.name;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            yield return new WaitForSecondsRealtime(textSpeed);
            dialogueText.text += letter;
            
        }
    }

    public void Update()
    {
        if (playerControls.Player.Jump.WasReleasedThisFrame() && animator.GetBool("isOpen") == true)
        {
            DisplayNextSentence();
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        playerControls.Player.Jump.Disable();

    }
}
