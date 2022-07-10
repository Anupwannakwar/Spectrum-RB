using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePlatform : MonoBehaviour
{
    private bool FadeIn, FadeOut;
    [SerializeField] private float FadeSpeed;
    public float Platform_UpTime;
    public float Platform_DownTime;

    public float StartDelay;

    private BoxCollider2D PlatformCollider;

    private void Start()
    {
        PlatformCollider = GetComponent<BoxCollider2D>();
        FadeIn = true;
    }

    void Update()
    {
        if(StartDelay >= 0)
            StartDelay -= Time.deltaTime;

        if (StartDelay <= 0)
        {
            if (FadeIn)
            {
                Color PlatformColor = this.GetComponent<SpriteRenderer>().color;
                float FadeAmount = PlatformColor.a + (FadeSpeed * Time.deltaTime);

                PlatformColor = new Color(PlatformColor.r, PlatformColor.g, PlatformColor.b, FadeAmount);
                this.GetComponent<SpriteRenderer>().color = PlatformColor;

                if (PlatformColor.a >= 0.7f)
                    PlatformCollider.isTrigger = false;

                if (PlatformColor.a >= 1)
                {
                    StartCoroutine(TriggerFadeOut());
                }
            }

            if (FadeOut)
            {
                Color PlatformColor = this.GetComponent<SpriteRenderer>().color;
                float FadeAmount = PlatformColor.a - (FadeSpeed * Time.deltaTime);

                PlatformColor = new Color(PlatformColor.r, PlatformColor.g, PlatformColor.b, FadeAmount);
                this.GetComponent<SpriteRenderer>().color = PlatformColor;

                if (PlatformColor.a <= 0.5f)
                    PlatformCollider.isTrigger = true;

                if (PlatformColor.a <= 0)
                {
                    StartCoroutine(TriggerFadeIn());
                }
            }
        }    
    }

    public IEnumerator TriggerFadeIn()
    {
        yield return new WaitForSeconds(Platform_DownTime);
        FadeOut = false;
        FadeIn = true;
    }
    public IEnumerator TriggerFadeOut()
    {
        yield return new WaitForSeconds(Platform_UpTime);
        FadeIn = false;
        FadeOut = true;
    }
}
