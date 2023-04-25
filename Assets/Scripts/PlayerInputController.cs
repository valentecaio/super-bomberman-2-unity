using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// manages player movement controls and animations
public class PlayerInputController : MonoBehaviour
{
    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer SpriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    [Header("Controls")]
    public InputActionMap inputActionMap;

    [HideInInspector]
    public Vector2 direction = Vector2.down;
    private PlayerStatus player;

    private void Awake()
    {
        activeSpriteRenderer = spriteRendererDown;
        player = gameObject.GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (inputActionMap.FindAction("MoveUp").IsPressed()) {
            setDirection(Vector2.up, spriteRendererUp);
        } else if (inputActionMap.FindAction("MoveDown").IsPressed()) {
            setDirection(Vector2.down, spriteRendererDown);
        } else if (inputActionMap.FindAction("MoveLeft").IsPressed()) {
            setDirection(Vector2.left, spriteRendererLeft);
        } else if (inputActionMap.FindAction("MoveRight").IsPressed()) {
            setDirection(Vector2.right, spriteRendererRight);
        } else {
            setDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        float speed = gameObject.GetComponent<PlayerStatus>().speed;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + translation);
    }

    public IEnumerator die()
    {
        this.enabled = false;
        this.GetComponent<PlayerBombController>().enabled = false;
        this.GetComponent<PlayerStatus>().enabled = false;
        this.spriteRendererUp.enabled = false;
        this.spriteRendererDown.enabled = false;
        this.spriteRendererLeft.enabled = false;
        this.spriteRendererRight.enabled = false;
        this.SpriteRendererDeath.enabled = true;
        yield return new WaitForSeconds(1.25f);
        this.gameObject.SetActive(false);
        FindObjectOfType<GameManager>().checkWinState();
    }

    private void setDirection(Vector2 newDirection, AnimatedSpriteRenderer spriteRenderer)
    {
        direction = newDirection;
        activeSpriteRenderer = spriteRenderer;

        activeSpriteRenderer.idle = (direction == Vector2.zero);

        spriteRendererUp.enabled    = (spriteRenderer == spriteRendererUp);
        spriteRendererDown.enabled  = (spriteRenderer == spriteRendererDown);
        spriteRendererLeft.enabled  = (spriteRenderer == spriteRendererLeft);
        spriteRendererRight.enabled = (spriteRenderer == spriteRendererRight);
    }

    private void OnEnable()
    {
        inputActionMap.Enable();
    }
}
