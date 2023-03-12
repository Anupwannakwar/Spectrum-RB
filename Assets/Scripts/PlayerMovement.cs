using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public static PlayerMovement instance;

    public Animator anim;
    public SpectrumBar SB;
    public LadderMovement LM;

    public HealthBar healthbar;

    //player Movement
    public float horizontal;
 
    public float RunSpeed;
    public float WalkSpeed;
    private float CurrentSpeed;

    bool jump = false;
    bool Crouch = false;
    bool isWalking = false;

    public bool isRunning
    {
        get
        {
            return horizontal != 0;
        }
        set
        {
            isRunning = false;
        }
    }


    //player shoot
    [SerializeField] private BulletPooling bulletList = null;
    [SerializeField] Transform ShootPoint = null;
    [SerializeField] Transform ShootPoint2 = null;
    [SerializeField] Transform ShootPoint3 = null;
    private float timeBetweenShots;
    public float StartTimeBtwShots;

    [SerializeField] private float ChargeSpeed;
    [SerializeField] private float ChargeTime;
    public bool isCharging;

    public GameObject chargeEffect;
    private GameObject charge;

    private bool instantiateCharge = true;

    //player health
    private const int MAXHEALTH = 500;
    private float m_health;

    private void OnEnable()
    {
        EventManager.Instance.OnUpdateHealth += UpdatePlayerHealth;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnUpdateHealth -= UpdatePlayerHealth;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timeBetweenShots = StartTimeBtwShots;

        m_health = MAXHEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        //Running and Walking

        if (Input.GetKeyDown(KeyCode.LeftShift) && horizontal != 0)
            isWalking = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            isWalking = false;

        if (isWalking == true)
        {
            CurrentSpeed = WalkSpeed;
        }
        else
            CurrentSpeed = RunSpeed;

        if(Crouch == false)
        {
            horizontal = Input.GetAxis("Horizontal") * CurrentSpeed;
        }

        if (anim.GetBool("IsShooting") == true && anim.GetFloat("Speed") < 0.01)
        {
            horizontal = 0;
        }

        if (!isWalking && controller.m_Grounded)
        {
            anim.SetBool("IsWalking", false);
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        else if (isWalking)
        {
            anim.SetFloat("Speed", 0);
            anim.SetBool("IsWalking", true);
        }
           
       
        //Jumping and Crouching
        if(Input.GetButtonDown("Jump") && controller.m_Grounded)
        {
            jump = true;
            anim.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown("Crouch") && horizontal == 0 && !LM.isClimbing)
        {
            Crouch = true;
            anim.SetBool("IsCrouching", true);
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            Crouch = false;
            anim.SetBool("IsCrouching", false);
        }

        //Shooting
        if (Input.GetMouseButton(0) && ChargeTime < 2)
        {
            isCharging = true;
            if (isCharging == true)
            {
                ChargeTime += Time.deltaTime * ChargeSpeed;
                anim.SetBool("IsShooting", true);
                if(ChargeTime > 1 && instantiateCharge)
                {
                    InstantiateChargedEffect();
                    instantiateCharge = false;
                }
            }
            else
            {
                destroyChargedEffect();
            }
        }
        if (timeBetweenShots <= 0)
        {      
            if(Input.GetMouseButtonUp(0) && ChargeTime >= 2)
            {
                SoundManager.instance.PlaySound("charged");
                ChargedShot();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                SoundManager.instance.PlaySound("shoot");
                anim.SetBool("IsShooting", true);
                Shoot();
                timeBetweenShots = StartTimeBtwShots;
                isCharging = false;
                instantiateCharge = true;
                destroyChargedEffect();
            }
            else
            {
                if(isCharging == false)
                    anim.SetBool("IsShooting", false);
            }
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }

        //special shot
        if(GameManager.instance.SpecialReady == true && Input.GetMouseButtonDown(1) && isCharging == false && GameManager.instance.SpecialAbilityActive)
        {
            anim.SetBool("IsShooting", true);
            SoundManager.instance.PlaySound("charged");
            ShootSpecial();
            GameManager.instance.healthBar.setPowerBar(0);
            GameManager.instance.SpecialReady = false;   
        }

        //changing Spectrum
        if(Input.GetKeyDown(KeyCode.E) && SB.currentRedValue > 0)
        {
            GameManager.instance.RedActive = !GameManager.instance.RedActive;
            GameManager.instance.SetRedActive();
            SB.startRedBar();
        }
        if (Input.GetKeyDown(KeyCode.Q) && SB.currentBlueValue > 0)
        {
            GameManager.instance.BlueActive = !GameManager.instance.BlueActive;
            GameManager.instance.SetBlueActive();
            SB.startBlueBar();
        }
    }

    public void OnLanding()
    {
        anim.SetBool("IsJumping", false);
        Debug.Log("landing");
    }

    private void FixedUpdate()
    {
        controller.Move(horizontal * Time.fixedDeltaTime, Crouch, jump);
        jump = false;
    }

    public void Shoot()
    {
        GameObject bullet = bulletList.GetBullet();
        if(!Crouch)
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint;
        }
        else
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint2;
        }
        if(horizontal != 0 && !isWalking)
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint3;
        }
        bullet.GetComponent<BulletScript>().ShotByPlayer = true;
        bullet.GetComponent<BulletScript>().FacingRight = controller.m_FacingRight;
        bullet.SetActive(true);

        ChargeTime = 0;
    }

    public void ChargedShot()
    {
        GameObject bullet = bulletList.GetChargedBullet();
        if (!Crouch)
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint;
        }
        else
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint2;
        }
        if (horizontal != 0 && !isWalking)
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint3;
        }
        bullet.GetComponent<BulletScript>().ShotByPlayer = true;
        bullet.GetComponent<BulletScript>().FacingRight = controller.m_FacingRight;
        bullet.SetActive(true);

        destroyChargedEffect();

        isCharging = false;
        instantiateCharge = true;
        ChargeTime = 0;
        anim.SetBool("IsShooting", false);
    }

    public void ShootSpecial()
    {
        GameObject bullet = bulletList.GetSpecialBullet();
        if (!Crouch)
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint;
        }
        else
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint2;
        }
        if (horizontal != 0 && !isWalking)
        {
            bullet.GetComponent<BulletScript>().BulletHolder = ShootPoint3;
        }
        bullet.GetComponent<BulletScript>().ShotByPlayer = true;
        bullet.GetComponent<BulletScript>().FacingRight = controller.m_FacingRight;
        bullet.SetActive(true);

        ChargeTime = 0;
        anim.SetBool("IsShooting", false);
    }

    public void InstantiateChargedEffect()
    {
        if (!Crouch)
        {
           charge = Instantiate(chargeEffect, ShootPoint);
        }
        else
        {
           charge = Instantiate(chargeEffect, ShootPoint2);
        }
        if (horizontal != 0 && !isWalking)
        {
            destroyChargedEffect();
           charge = Instantiate(chargeEffect, ShootPoint3);
        }
    }

    public void destroyChargedEffect()
    {
        Destroy(charge);
    }

    public void UpdatePlayerHealth(float health)
    {
        //Changing health
        m_health += health;

        //If player hurt
        if (health < 0)
        {
            SoundManager.instance.PlaySound("hurt");
            anim.SetTrigger("IsHurt");
            Debug.Log(m_health);

            if (m_health <= 0)
            {
                GameManager.instance.gameover = true;
                gameObject.SetActive(false);
            }
        }

        healthbar.setHealth(m_health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Spike"))
        {
            EventManager.Instance.UpdateHealth(-20f);
        }
        if (collision.CompareTag("DeathBound"))
        {
            gameObject.SetActive(false);
            GameManager.instance.gameover = true;
        }
    }
}
