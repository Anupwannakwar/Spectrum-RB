using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeTrigger : MonoBehaviour
{
    public Dialouge dialouge;

    public Animator anim;

    private void Start()
    {
        anim.SetBool("isIdle", true);
    }

    public void TriggerDialouge()
    {
        DialougeManager.instance.StartDialouge(dialouge);
    }

    [SerializeField] private Rigidbody2D rb;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.T) && collision.gameObject.tag == "Player")
        {
            TriggerDialouge();
        }
    }
}