using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Striker_Run : StateMachineBehaviour
{

    public float speed = 2.5f;
    public float SlashRange = 4f;
    public float ShootRange = 7f;

    private bool Shoot = false;
    private bool Dash = false;

    Transform player;
    Rigidbody2D rb;
    Striker striker;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        striker = animator.GetComponent<Striker>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(player.position, rb.position) <= SlashRange)
        {
            GetRandomDash();
        }
        if (!Dash)
        {
            striker.LookAtPlayer();

            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(player.position, rb.position) <= SlashRange)
            {
                //slash attack
                GetShootOrSlash();
                if (!Shoot)
                {
                    animator.SetTrigger("Slash");
                }
                Shoot = false;
            }
            if (Vector2.Distance(player.position, rb.position) <= ShootRange)
            {
                //shoot attack
                GetShootOrSlash();
                if (Shoot)
                {
                    animator.SetTrigger("Shoot");
                }
                Shoot = false;
            }
        }
        else
        {        
            Vector2 DashPos = rb.transform.position;
            if(!striker.isFlipped)
            {
                DashPos.x = player.position.x - 2.5f;
            }
            else
            {
                DashPos.x = player.position.x + 2.5f; ;
            }           
            rb.MovePosition(DashPos);
            animator.SetTrigger("Dash");
            Dash = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Slash");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Dash");
    }

    public void GetShootOrSlash()
    {
        int i = Random.Range(1, 7);
        if(i == 1)
        {
            Shoot = true;
        }
        else
        {
            Shoot = false;
        }
    }
    public void GetRandomDash()
    {
        int i = Random.Range(1, 5);
        bool playerShooting = player.GetComponent<PlayerMovement>().anim.GetBool("IsShooting");
        if (i == 1 && playerShooting)
        {
            Dash = true;
        }
        else
        {
            Dash = false;
        }
    }
}
