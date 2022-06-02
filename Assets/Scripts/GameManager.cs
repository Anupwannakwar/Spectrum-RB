using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool RedActive = false;
    public bool BlueActive = false;

    public GameObject RedPanel;
    public GameObject BluePanel;

    public bool isConversing = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if(isConversing)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                DialougeManager.instance.DisplayNextSentence();
            }
        }
    }

    public void SetRedActive()
    {
        if(RedActive == true)
        {
            RedPanel.SetActive(true);
            BluePanel.SetActive(false);
            BlueActive = false;
        }
        else
        {
            RedPanel.SetActive(false);
        }
             
    }

    public void SetBlueActive()
    {
        if(BlueActive == true)
        {
            BluePanel.SetActive(true);
            RedPanel.SetActive(false);
            RedActive = false;
        }
        else
        {
            BluePanel.SetActive(false);
        }     
    }
}
