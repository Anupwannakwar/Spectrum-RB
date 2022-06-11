using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Striker : MonoBehaviour
{
    public Transform player;

    public bool isFlipped = true;

    public Animator anim;

    //for attacking
    public Transform EnemyattackPos;
    public Transform EnemyShootPos;
    public float EnemyattackRangeX;
    public float EnemyattackRangeY;
    public float EnemyShootRangeX;
    public float EnemyShootRangeY;
    public LayerMask whatIsPlayer;
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    private void Start()
    {
        anim.SetBool("StartFight", true);
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;

        flipped.z *= -1f;

        if(transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if(transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    private void attackPlayerSlash()
    {
        Collider2D[] ToHit = Physics2D.OverlapBoxAll(EnemyattackPos.position, new Vector2(EnemyattackRangeX, EnemyattackRangeY), 0, whatIsPlayer);
        for (int i = 0; i < ToHit.Length; i++)
        {
            EventManager.Instance.UpdateHealth(-15);
        }         
    }

    private void attackPlayerShoot()
    {       
        Collider2D[] ToHit = Physics2D.OverlapBoxAll(EnemyShootPos.position, new Vector2(EnemyShootRangeX, EnemyShootRangeY), 0, whatIsPlayer);
        for (int i = 0; i < ToHit.Length; i++)
        {
            EventManager.Instance.UpdateHealth(-25);
        }    
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(EnemyattackPos.position, new Vector3(EnemyattackRangeX, EnemyattackRangeY, 0));
        Gizmos.DrawWireCube(EnemyShootPos.position, new Vector3(EnemyShootRangeX, EnemyShootRangeY, 0));
    }
}
