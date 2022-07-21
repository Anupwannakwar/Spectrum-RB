using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance { get { return instance; } }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    //Score Event
    public event UnityAction<int> OnUpdateScore;

    public void UpdateScore(int num)
    {
        if (OnUpdateScore != null)
        {
            OnUpdateScore(num);
        }
    }

    //Player Health Event
    public event UnityAction<float> OnUpdateHealth;

    public void UpdateHealth(float health)
    {
        if(OnUpdateHealth != null)
        {
            OnUpdateHealth(health);

        }
    }
}
