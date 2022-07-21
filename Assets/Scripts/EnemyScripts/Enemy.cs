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
        Striker,
        MechUnit,
        BipedalUnit,
        SpaceMarine,
        BotWheel,
        turret
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

        if(GameManager.instance.SpecialAbilityActive)
        {
            if (GameManager.instance.healthBar.slider.value <= 10)
            {
                GameManager.instance.healthBar.setPowerBar((int)GameManager.instance.healthBar.slider.value + 1);
            }
        }
       
        if (enemyName == EnemyName.Striker)
        {
            healthbar.setBossHealth(BaseHealth);
        }
        else
        {
            if(enemyName == EnemyName.DroidZapper || enemyName == EnemyName.BotWheel)
                anim.SetTrigger("IsHurt");
        }
       
        if (BaseHealth <= 0)
        { 
            if (enemyName == EnemyName.SentryDrone || enemyName == EnemyName.BipedalUnit || enemyName == EnemyName.MechUnit || enemyName == EnemyName.turret)
                StartCoroutine(Destroyed());
            else if((enemyName == EnemyName.BotWheel || enemyName == EnemyName.SpaceMarine || enemyName == EnemyName.DroidZapper))
                StartCoroutine(Dead());
            else
                Destroy(this.gameObject);
        }
    }

    public IEnumerator Destroyed()
    {
        if (enemyName == EnemyName.turret)
            anim.SetBool("Shoot", false);

        anim.SetBool("Destroyed", true);
        yield return new WaitForSeconds(0.45f);
        Destroy(this.gameObject);
    }

    public IEnumerator Dead()
    {
        anim.SetBool("Dead", true);
        yield return new WaitForSeconds(0.45f);
        Destroy(this.gameObject);
    }
}
