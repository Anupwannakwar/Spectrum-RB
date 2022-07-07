using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry_Drone : MonoBehaviour
{
    public float speed;
    public float PatrolSpeed;
    private GameObject player;
    public bool chase = false;
    public Animator anim;
    public float stopingDistance = 1.0f;
    public Patrol patrol;
    private Rigidbody2D rb;
    private float dirx = -1;

    //detection
    public float DetectRangeX;
    public float DetectRangeY;
    public LayerMask whatIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        chase = isPlayerFound();

        if(chase == true)
        {
            FlipDrone();
            ChasePlayer();
        }
        else
        {
            if (patrol.isFacingRight)
            {
                dirx = 1;
            }
            else
            {
                dirx = -1;
            }
            rb.velocity = Vector2.right * dirx * PatrolSpeed;
            patrol.patrol();
        }    
    }

    private void ChasePlayer()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= stopingDistance)
        {
            //shoot player
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    public void FlipDrone()
    {
        if (transform.position.x > player.transform.position.x)
        {
            anim.SetBool("Turn", false);
        }
        else
        {
            anim.SetBool("Turn", true);
        }
    }

    private bool isPlayerFound()
    {
        Collider2D[] ToDetect = Physics2D.OverlapBoxAll(transform.position, new Vector2(DetectRangeX, DetectRangeY), 0, whatIsPlayer);
        for (int i = 0; i < ToDetect.Length; i++)
        {
            if(ToDetect[i].GetComponent<PlayerMovement>() != null)
            {
                return true;
            }      
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(DetectRangeX, DetectRangeY, 0));
    }
}
