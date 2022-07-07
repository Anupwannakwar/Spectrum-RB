using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int BaseHealth;

    [SerializeField]
    private Animator anim;

    public HealthBar healthbar;

    public enum EnemyName
    {
        DroidZapper,
        SentryDrone,
        Striker
    }

    public EnemyName enemyName;

    private void Start()
    {
        if(enemyName == EnemyName.Striker)
        {
            healthbar.setMaxHealth(BaseHealth);
        }
    }

    public void takeDamage(int Damage)
    {
        BaseHealth -= Damage;

        if(enemyName == EnemyName.Striker)
        {
            healthbar.setBossHealth(BaseHealth);
        }
        else
        {
            if(enemyName == EnemyName.DroidZapper)
                anim.SetTrigger("IsHurt");
        }
       
        if (BaseHealth <= 0)
        { 
            if (enemyName == EnemyName.SentryDrone)
                StartCoroutine(Destroyed());
            else
                Destroy(this.gameObject);
        }
    }

    public IEnumerator Destroyed()
    {
        anim.SetBool("Destroyed", true);
        yield return new WaitForSeconds(0.35f);
        Destroy(this.gameObject);
    }
}