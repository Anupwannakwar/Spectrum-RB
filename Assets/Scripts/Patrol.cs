using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    private Vector3 characterIntialPos;
    public float patrolingRange;
    public bool isFacingRight = false;

    private Sentry_Drone sd;

    void Start()
    {
        characterIntialPos = transform.position;
        sd = GetComponent<Sentry_Drone>();
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        if(sd != null)
        {
            sd.anim.SetBool("Turn", isFacingRight);
        }
        else
        {
            transform.Rotate(0, 180, 0);
        }
    }
    public void patrol()
    {
        if (transform.position.x > characterIntialPos.x + patrolingRange && isFacingRight) Flip();
        if (transform.position.x < characterIntialPos.x - patrolingRange && !isFacingRight) Flip();
    }
}
