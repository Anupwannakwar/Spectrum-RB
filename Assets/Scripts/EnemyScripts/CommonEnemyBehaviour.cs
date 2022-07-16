using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnemyBehaviour : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private float dirx = -1;
    public Patrol patrol;
    public bool isSpectrumR;
    public bool isSpectrumB;

    //for attacking
    public Transform EnemyattackPos;
    public float EnemyattackRangeX;
    public float EnemyattackRangeY;
    public LayerMask whatIsPlayer;
    public int damage;
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    //detecting
    public float detectRange;

    //following
    public Transform target;
    public float stopingDistance;

    private bool isFollowing = false;

    private Animator anim;

    //effects
    public GameObject ForceField;
    public SpriteRenderer spriter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (patrol.isFacingRight)
        {
            dirx = 1;
        }
        else
        {
            dirx = -1;
        }

        //detecting
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (distanceToPlayer < detectRange)
        {
            followPlayer();
            isFollowing = true;
        }
        else isFollowing = false;

        Move();

        if (isSpectrumR)
        {
            if (GameManager.instance.RedActive == true)
            {
                spriter.enabled = true;
            }
            else
            {
                spriter.enabled = false;
            }
        }

        else if (isSpectrumB)
        {
            if (GameManager.instance.BlueActive == true)
            {
                spriter.enabled = true;
            }
            else
            {
                spriter.enabled = false;
            }
        }
        else
        {
            spriter.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (!isFollowing)
        {
            patrol.patrol();
        }
    }

    private void Move()
    {
        if (patrol.isFacingRight && isFollowing && target.position.x < transform.position.x + stopingDistance)
        {
            rb.velocity = Vector2.zero;
            attackPlayer();
        }
        else if (!patrol.isFacingRight && isFollowing && target.position.x > transform.position.x - stopingDistance)
        {
            rb.velocity = Vector2.zero;
            attackPlayer();
        }
        else
        {
            switch (GetComponent<Enemy>().enemyName)
            {
                case Enemy.EnemyName.DroidZapper:
                    anim.SetBool("IsAttacking", false);
                    break;
                case Enemy.EnemyName.SentryDrone:
                    break;
                case Enemy.EnemyName.Striker:
                    break;
                case Enemy.EnemyName.MechUnit:
                    break;
                case Enemy.EnemyName.BipedalUnit:
                    break;
                case Enemy.EnemyName.SpaceMarine:
                    break;
                case Enemy.EnemyName.BotWheel:
                    break;
                default:
                    break;
            }
            
            rb.velocity = Vector2.right * dirx * moveSpeed;
        }
    }

    public void followPlayer()
    {
        //following
        if (target.position.y < transform.position.y + 2.0f)
        {
            if (target.position.x > transform.position.x && !patrol.isFacingRight) patrol.Flip();
            if (target.position.x < transform.position.x && patrol.isFacingRight) patrol.Flip();
        }
    }

    private void attackPlayer()
    {
        int damage = -5;

        if (timeBtwAttack <= 0)
        {
            switch (GetComponent<Enemy>().enemyName)
            {
                case Enemy.EnemyName.DroidZapper:
                    anim.SetBool("IsAttacking", true);
                    damage = -5;
                    break;
                case Enemy.EnemyName.SentryDrone:
                    break;
                case Enemy.EnemyName.Striker:
                    break;
                case Enemy.EnemyName.MechUnit:
                    damage = -15;
                    ForceField.SetActive(true);
                    break;
                case Enemy.EnemyName.BipedalUnit:
                    damage = -15;
                    ForceField.SetActive(true);
                    break;
                case Enemy.EnemyName.SpaceMarine:
                    break;
                case Enemy.EnemyName.BotWheel:
                    break;
                default:
                    break;
            }
            
            Collider2D[] ToHit = Physics2D.OverlapBoxAll(EnemyattackPos.position, new Vector2(EnemyattackRangeX, EnemyattackRangeY), 0, whatIsPlayer);
            for (int i = 0; i < ToHit.Length; i++)
            {
                EventManager.Instance.UpdateHealth(damage);
                Debug.Log("Player Damaged");
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.DrawWireCube(EnemyattackPos.position, new Vector3(EnemyattackRangeX, EnemyattackRangeY, 0));
    }
}
