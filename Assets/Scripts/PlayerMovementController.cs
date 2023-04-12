using System.Collections;
using UnityEngine;

// manages player movement controls and animations
public class PlayerMovementController : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    private Vector2 direction = Vector2.down;

    [Header("Controls")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    [Header("Sprites")]
    public AnimatedSpriteRenderer spriteRendererUp;
    public AnimatedSpriteRenderer spriteRendererDown;
    public AnimatedSpriteRenderer spriteRendererLeft;
    public AnimatedSpriteRenderer spriteRendererRight;
    public AnimatedSpriteRenderer SpriteRendererDeath;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    private void Start()
    {
        activeSpriteRenderer = spriteRendererDown;
    }

    private void Update()
    {
        if (Input.GetKey(inputUp)) {
            setDirection(Vector2.up, spriteRendererUp);
        } else if (Input.GetKey(inputDown)) {
            setDirection(Vector2.down, spriteRendererDown);
        } else if (Input.GetKey(inputLeft)) {
            setDirection(Vector2.left, spriteRendererLeft);
        } else if (Input.GetKey(inputRight)) {
            setDirection(Vector2.right, spriteRendererRight);
        } else {
            setDirection(Vector2.zero, activeSpriteRenderer);
        }
    }

    private void FixedUpdate()
    {
        float speed = gameObject.GetComponent<PlayerStatus>().speed;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + translation);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Explosion")) {
            StartCoroutine(die());
        }
    }

    private IEnumerator die()
    {
        this.enabled = false;
        this.GetComponent<PlayerBombController>().enabled = false;
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
}
