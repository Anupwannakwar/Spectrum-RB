using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Transform player;

    public bool facingRight;

    private float bulletSpeed = 10;
    private bool Bullethit = false;

    public CircleCollider2D col;

    private void Awake()
    {
        this.enabled = true;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.x < transform.position.x)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }

        if (!Bullethit)
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

        transform.position = transform.parent.position;
        Bullethit = false;
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>() != null)
            {
                EventManager.Instance.UpdateHealth(-2.5f);
                col.enabled = false;
                DisableBulletAfterHittingEnemy();
            }
        }
    }

    private void DisableBulletAfterHittingEnemy()
    {
        Bullethit = true;
        GetComponent<Animator>().SetBool("Destroy", true);
        StartCoroutine(DeactivateBullet(0.2f));
    }
}
