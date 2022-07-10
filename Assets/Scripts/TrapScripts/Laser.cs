using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float timeTilDestroy;
    public float length = 2f;

    public enum LaserType
    {
        Red,
        Blue,
        Normal
    }

    public LaserType Type;

    private void Start()
    {
        this.transform.localScale = new  Vector3(this.transform.localScale.x , length , this.transform.localScale.z);
    }

    private void Update()
    {
        Destroy(gameObject, timeTilDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(!GameManager.instance.RedActive && Type == LaserType.Red)
            {
                EventManager.Instance.UpdateHealth(-15);
            }
            else if(!GameManager.instance.BlueActive && Type == LaserType.Blue)
            {
                EventManager.Instance.UpdateHealth(-15);
            }
            else if(Type == LaserType.Normal)
            {
                EventManager.Instance.UpdateHealth(-15);
            }
           
        }
    }
}
