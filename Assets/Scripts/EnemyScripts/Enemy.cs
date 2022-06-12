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
            anim.SetTrigger("IsHurt");
        }
       
        if (BaseHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
