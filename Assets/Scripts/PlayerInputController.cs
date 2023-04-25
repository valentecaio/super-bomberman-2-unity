using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

// manages player input controls and animations
public class PlayerInputController : MonoBehaviour
{
    [Header("Bombs")]
    public Tilemap destructibleTilemap;
    public GameObject bombPrefab;

    [Header("Movement Sprites")]
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
    private Player player;

    private void Awake()
    {
        activeSpriteRenderer = spriteRendererDown;
        player = gameObject.GetComponent<Player>();
    }

    private void Update()
    {
        // movement
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

        // bomb
        if (inputActionMap.FindAction("PlaceBomb").IsPressed() && player.bombs.Count < player.bombAmount) {
            placeBomb();
        } else if (inputActionMap.FindAction("DetonateBomb").IsPressed()) {
            foreach (GameObject bomb in player.bombs) {
                if (bomb.GetComponent<Bomb>().type == BombType.RemoteControl) {
                    bomb.GetComponent<Bomb>().bombExplode();
                    break;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        float speed = gameObject.GetComponent<Player>().speed;
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + translation);
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

    private void placeBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // we can't place a bomb over a bomb or wall
        if (Physics2D.OverlapBox(position, Vector2.one/2f, 0f, LayerMask.GetMask("Bomb", "SoftBlock"))) {
            return;
        }

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bomb.GetComponent<Bomb>().init(player, destructibleTilemap);
        player.bombs.Add(bomb);

        // we ignore this collision until the player exits the bomb sprite
        player.droppingBomb = true;
        Physics2D.IgnoreCollision(bomb.GetComponent<CircleCollider2D>(), gameObject.GetComponent<CircleCollider2D>());
    }

    private void OnEnable()
    {
        inputActionMap.Enable();
    }
}
