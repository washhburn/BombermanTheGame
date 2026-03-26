using Unity.VisualScripting;
using UnityEngine;

public class AnimatedSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite idleSprite;
    public Sprite[] animationSprites; // En array som innehåller alla sprites som används i animationen

    public float animationSpeed = 0.25f; // Tiden mellan varje frame i animationen
    private int animationFrame;

    public bool loop = true;
    public bool idle = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() // När GameObject aktiveras, se till att SpriteRenderer är synlig och att opaciteten är 100%
    {
        spriteRenderer.enabled = true;

        Color c = spriteRenderer.color;
        c.a = 1f;
        spriteRenderer.color = c;
    }

    private void OnDisable() // När GameObject inaktiveras, dölja SpriteRenderer för att säkerställa att det inte syns när det inte ska
    {
        spriteRenderer.enabled = false;
    }

    private void Start() //Starta animationen när spelet startar
    {
        InvokeRepeating(nameof(NextFrame), animationSpeed, animationSpeed);
    }

    private void NextFrame() //En metod som hanterar att byta till nästa frame i animationen
    {
        animationFrame++;
        if (loop && animationFrame >= animationSprites.Length)
        {
            animationFrame = 0;
        }

        if (idle)
        {
            spriteRenderer.sprite = idleSprite;
        }
        else if (animationFrame >= 0 && animationFrame < animationSprites.Length)
        {
            spriteRenderer.sprite = animationSprites[animationFrame];
        }
    }

    public void PlayOnce() //En metod som startar animationen från början och spelar den en gång utan att loopa
    {
        loop = false;
        idle = false;
        animationFrame = -1; // Starta animationen från början
        gameObject.SetActive(true); // Se till att GameObject är aktivt så att animationen kan spelas
        CancelInvoke(nameof(NextFrame)); // Stoppa eventuella tidigare anrop av NextFrame
        InvokeRepeating(nameof(NextFrame), 0f, animationSpeed); // Starta NextFrame direkt och fortsätt med animationSpeed intervall
    }
}
