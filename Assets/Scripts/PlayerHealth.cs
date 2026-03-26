using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int lives = 3; //antalet liv som spelaren har
    public float invincibilityTime = 2f; //tiden i sekunder som spelaren är osårbar efter att ha tagit skada
    private bool isInvincible = false; //en flagga som indikerar om spelaren är osårbar eller inte
    private Coroutine blinkCoroutine; //en referens till den coroutine som hanterar blinkningen när spelaren är osårbar
    private AnimatedSpriteRenderer[] allSprites;

    private void Start()
    {
        allSprites = GetComponentsInChildren<AnimatedSpriteRenderer>(); //hämtar alla AnimatedSpriteRenderer-komponenter som är barn till spelaren
    }

    public void LoseLife() //en metod som hanterar när spelaren förlorar ett liv
    {
        if (!isInvincible) //kontrollerar om spelaren inte är osårbar
        {
            lives--; //minskar antalet liv med 1
            if (lives <= 0) //kontrollerar om spelaren har förlorat alla liv
            {
                lives = 0;
                Die();
            }
            else
            {
                isInvincible = true;

                if (blinkCoroutine != null)
                    StopCoroutine(blinkCoroutine);
                blinkCoroutine = StartCoroutine(Blink()); //startar en coroutine för att få spelaren att blinka när den är osårbar
                Invoke("BecomeVulnerable", this.invincibilityTime); //använder Invoke för att kalla på metoden BecomeVulnerable efter invincibilityTime sekunder, vilket gör spelaren sårbar igen
            }
        }
    }

    private void Die()
    {
        MovementController mc = GetComponent<MovementController>();
        mc.enabled = false;
        GetComponent<BombController>().enabled = false;

        mc.spriteRendererUp.enabled = false;
        mc.spriteRendererDown.enabled = false;
        mc.spriteRendererLeft.enabled = false;
        mc.spriteRendererRight.enabled = false;
        mc.spriteRendererDeath.enabled = true;

        Invoke(nameof(OnDeathSequenceEnd), 1.5f);
    }

    private void OnDeathSequenceEnd()
    {
        gameObject.SetActive(false);
    }

    private void BecomeVulnerable() //en metod som gör spelaren sårbar igen efter att ha varit osårbar
    {
        isInvincible = false; //sätter isInvincible till false så att spelaren kan ta skada igen
    }

    IEnumerator Blink()
    {
        float timer = 0f;
        float interval = 0.1f;

        while (timer < invincibilityTime)
        {
            foreach (var anim in allSprites)
            {
                if (anim != null)
                {
                    SpriteRenderer sr = anim.GetComponent<SpriteRenderer>();
                    
                    if (!sr.enabled) continue; //om sprite-renderern inte är aktiverad, hoppa över den

                    Color c = sr.color;
                    c.a = (c.a == 1f) ? 0.3f : 1f;
                    sr.color = c;
                }
            }
            yield return new WaitForSeconds(interval); //väntar i interval sekunder innan nästa blinkning
            timer += interval; //ökar timer med interval så att loopens totala varaktighet inte överstiger invincibilityTime
        }

        foreach (var anim in allSprites)
        {
            if (anim != null)
            {
                SpriteRenderer sr = anim.GetComponent<SpriteRenderer>();

                if (!sr.enabled) continue; //om sprite-renderern inte är aktiverad, hoppa över den

                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
        }
    }
}
