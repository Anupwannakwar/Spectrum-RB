using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Transform player;
    private Transform bulletHolder = null;

    public enum BulletType
    {
        Normal,
        Charged,
    }

    public BulletType bulletType;

    public Transform BulletHolder { set { bulletHolder = value; } }

    private bool facingRight;
    public bool FacingRight { set { facingRight = value; } }
    private bool shotByPlayer;
    public bool ShotByPlayer { set { shotByPlayer = value; } }

    private float bulletSpeed = 10;
    private bool Bullethit = false;

    private void Awake()
    {
        this.enabled = true;
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        if (bulletHolder != null)
            transform.position = bulletHolder.position;
        else
            transform.position = Vector3.zero;

        StartCoroutine(DeactivateBullet(3));
    }

    // Update is called once per frame
    void Update()
    {
        if(!Bullethit)
        {
            if (facingRight)
            {
                transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (!facingRight)
            {
                transform.Translate(-Vector3.right * Time.deltaTime * bulletSpeed);
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
        } 

        StartCoroutine(DeactivateBullet(5));
    }

    IEnumerator DeactivateBullet(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

       // gameObject.transform.SetParent(bulletHolder);
        transform.position = transform.parent.position;
        Bullethit = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && shotByPlayer)
        {
            if(collision.gameObject.GetComponent<CommonEnemyBehaviour>() != null)
            {
                if (GameManager.instance.RedActive && collision.gameObject.GetComponent<CommonEnemyBehaviour>().isSpectrumR)
                {
                    DealDamage(collision.gameObject.GetComponent<Enemy>());
                    DisableBulletAfterHittingEnemy();
                }
                if (GameManager.instance.BlueActive && collision.gameObject.GetComponent<CommonEnemyBehaviour>().isSpectrumB)
                {
                    DealDamage(collision.gameObject.GetComponent<Enemy>());
                    DisableBulletAfterHittingEnemy();
                }
                if(!collision.gameObject.GetComponent<CommonEnemyBehaviour>().isSpectrumR && !collision.gameObject.GetComponent<CommonEnemyBehaviour>().isSpectrumB)
                {
                    DealDamage(collision.gameObject.GetComponent<Enemy>());
                    DisableBulletAfterHittingEnemy();
                }
            }
            else if(collision.gameObject.GetComponent<Striker>() != null)
            {
                DealDamage(collision.gameObject.GetComponent<Enemy>());
                DisableBulletAfterHittingEnemy();
            }
            else if(collision.gameObject.GetComponent<Sentry_Drone>() != null)
            {
                DealDamage(collision.gameObject.GetComponent<Enemy>());
                DisableBulletAfterHittingEnemy();
            }
        }
    }

    private void DealDamage(Enemy enemy)
    {
        switch (bulletType)
        {
            case BulletType.Normal:
                enemy.takeDamage(5);
                break;
            case BulletType.Charged:
                enemy.takeDamage(15);
                break;
            default:
                enemy.takeDamage(5);
                break;
        }
    }

    private void DisableBulletAfterHittingEnemy()
    {
        Bullethit = true;
        GetComponent<Animator>().SetBool("Destroy", true);
        StartCoroutine(DeactivateBullet(0.2f));
    }
}
