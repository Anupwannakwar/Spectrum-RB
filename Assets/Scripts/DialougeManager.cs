using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialougeManager : MonoBehaviour
{
    private Queue<string> sentences;

    public static DialougeManager instance;

    public TMP_Text nameText;
    public TMP_Text dialougeText;

    public Animator anim;

    public Image RaeAvatar;
    public Image DroneAvatar;

    public bool toggleAvatar = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sentences = new Queue<string>();
        RaeAvatar.gameObject.SetActive(false);
        DroneAvatar.gameObject.SetActive(false);
    }

    public void StartDialouge (Dialouge dialouge)
    {
        GameManager.instance.isConversing = true;
        Debug.Log("Starting Conversation With - " + dialouge.NPC_name);

        anim.SetBool("IsOpen", true);

        nameText.text = dialouge.NPC_name;

        sentences.Clear();

        foreach (string sentence in dialouge.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialouge();
            return;
        }

        if(toggleAvatar == true)
        {
            RaeAvatar.gameObject.SetActive(false);
            DroneAvatar.gameObject.SetActive(true);
            nameText.text = "GIGA DRONE";
            toggleAvatar = false;
        }
        else
        {
            RaeAvatar.gameObject.SetActive(true);
            DroneAvatar.gameObject.SetActive(false);
            nameText.text = "RAE";
            toggleAvatar = true;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialougeText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialougeText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void EndDialouge()
    {
        GameManager.instance.isConversing = false;
        anim.SetBool("IsOpen", false);
        Debug.Log("End Of Conversation");
        GameManager.instance.SpecialAbilityActive = true;
    }


}
