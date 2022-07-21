using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public bool RedActive = false;
    public bool BlueActive = false;

    public static bool gameIsPaused = false;

    public GameObject RedPanel;
    public GameObject BluePanel;

    public GameObject HUD;
    public GameObject Options;
    public GameObject Pause;
    public GameObject gameoverPanel;

    public bool isConversing = false;
    public bool gameover = false;

    public int powerUpCounter = 0;
    public bool SpecialReady = false;
    public HealthBar healthBar;
    public Slider VolumeBar;
    public Toggle Mute;

    public bool SpecialAbilityActive = true;

    private void Awake()
    {
        if(instance == null)
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
        Time.timeScale = 1f;
        healthBar.setMaxPowerBar(10);
    }

    private void Update()
    {
        if(SpecialAbilityActive == false)
        {
            healthBar.gameObject.SetActive(false);
        }
        else
        {
            healthBar.gameObject.SetActive(true);
        }

        if (Mute.isOn)
        {
            PlayerPrefs.SetFloat("MusicVolume", 0);
            PlayerPrefs.SetFloat("EffectsVolume", 0);
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVolume", VolumeBar.value);
            PlayerPrefs.SetFloat("EffectsVolume", VolumeBar.value);
        }
       
        SoundManager.instance.effectVolumeChanged();
        SoundManager.instance.musicVolumeChanged();

        if (isConversing)
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                DialougeManager.instance.DisplayNextSentence();
            }
        }

        //pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }

        if(healthBar.slider.value >= 10)
        {
            SpecialReady = true;
        }

        if(gameover)
        {
            Time.timeScale = 0f;
            gameoverPanel.SetActive(true);
        }
    }

    public void SetRedActive()
    {
        if(RedActive == true)
        {
            RedPanel.SetActive(true);
            BluePanel.SetActive(false);
            BlueActive = false;
        }
        else
        {
            RedPanel.SetActive(false);
        }
             
    }

    public void SetBlueActive()
    {
        if(BlueActive == true)
        {
            BluePanel.SetActive(true);
            RedPanel.SetActive(false);
            RedActive = false;
        }
        else
        {
            BluePanel.SetActive(false);
        }     
    }

    public void Resume()
    {
        HUD.SetActive(true);
        Pause.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Paused()
    {
        HUD.SetActive(false);
        Pause.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void EnableOptions()
    {
        Pause.SetActive(false);
        Options.SetActive(true);
    }

    public void returnToPause()
    {
        Options.SetActive(false);
        Pause.SetActive(true);
    }
}
