using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    public Rigidbody2D rb;
    public float movementSpeed = 5f;
    private Vector2 direction = Vector2.down;

    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer spriteRendererDeath;

    private AnimatedSpriteRenderer currentSpriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpriteRenderer = spriteRendererDown; //sätter den nedåtriktade sprite-renderern som standard när spelet startar
    }

    private void Update()
    {
        if (Input.GetKey(upKey)) //kontrollerar om upKey (W) har tryckts ned
        {
            SetDirection(Vector2.up, spriteRendererUp);
        }
        else if (Input.GetKey(downKey))
        {
            SetDirection(Vector2.down, spriteRendererDown);
        }
        else if (Input.GetKey(leftKey))
        {
            SetDirection(Vector2.left, spriteRendererLeft);
        }
        else if (Input.GetKey(rightKey))
        {
            SetDirection(Vector2.right, spriteRendererRight);
        }
        else
        {
            SetDirection(Vector2.zero, currentSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        //flyttar spelaren i den aktuella riktningen med en hastighet som är baserad på movementSpeed & Time.fixedDeltaTime
        rb.MovePosition(rb.position + direction * movementSpeed * Time.fixedDeltaTime); 
        
    }

    private void SetDirection (Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer) 
    //uppdaterar riktningen och aktiverar den korrekta sprite-renderern baserat på den nya riktningen
    {
        direction = newDirection;
        spriteRendererUp.enabled = spriteRenderer == spriteRendererUp;
        spriteRendererDown.enabled = spriteRenderer == spriteRendererDown;
        spriteRendererLeft.enabled = spriteRenderer == spriteRendererLeft;
        spriteRendererRight.enabled = spriteRenderer == spriteRendererRight;

        currentSpriteRenderer = spriteRenderer;
        currentSpriteRenderer.idle = direction == Vector2.zero;
    }

}
