using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeTrigger : MonoBehaviour
{
    public Dialouge dialouge;

    public void TriggerDialouge()
    {
        DialougeManager.instance.StartDialouge(dialouge);
    }

    [SerializeField] private Rigidbody2D rb;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F) && collision.gameObject.tag == "Player")
        {
            TriggerDialouge();
        }
    }
}