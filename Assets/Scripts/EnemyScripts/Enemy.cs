using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int BaseHealth;

    [SerializeField]
    private Animator anim;

    public void takeDamage(int Damage)
    {
        BaseHealth -= Damage;
        if(anim.GetComponent<Striker>() == null)
        {
            anim.SetTrigger("IsHurt");
        }
       
        if (BaseHealth == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
