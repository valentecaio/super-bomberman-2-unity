using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    public float speed = 5f;
    public float bombTimer = 3f;
    public float explosionDuration = 0.85f;
    public Explosion explosionPrefab;
    public SoftBlock SoftBlockPrefab;

    private Vector2 direction = Vector2.zero;

    // these are passed from the player at bomb init time
    [HideInInspector]
    public BombType type;
    private ColourType colour;
    private int explosionLength = 2;
    private Tilemap destructibleTilemap;
    private PlayerStatus player;

    // must be called at bomb creation
    public void init(PlayerStatus player, Tilemap destructibleTilemap)
    {
        this.player = player;
        this.destructibleTilemap = destructibleTilemap;
        this.type = player.bombType;
        this.colour = player.colour;
        this.explosionLength = (player.bombType == BombType.PowerBomb) ? 99 : player.fireAmout;
        setSprites();
    }

    private void Start()
    {
        if (type != BombType.RemoteControl) {
            timedDetonation(bombTimer);
        }
    }

    // used to move bomb when kicked
    private void FixedUpdate()
    {
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + translation);
    }

    // select sprites according to bomb colour and type
    private void setSprites()
    {
        Sprite[] spriteArray = Resources.LoadAll<Sprite>("Bombs/" + System.Enum.GetName(typeof(ColourType), colour));
        int i = 0;
        if (type == BombType.Common) {
            i = 0;
        } else if (type == BombType.RemoteControl) {
            i = 4;
        } else if (type == BombType.PierceBomb) {
            i = 52;
        } else if (type == BombType.PowerBomb && !player.hasAPowerBombDeployed()) {
            i = 56;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[i];
        gameObject.GetComponent<AnimatedSpriteRenderer>().idleSprite = spriteArray[i];
        gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[0] = spriteArray[i+0];
        gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[1] = spriteArray[i+1];
        gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[2] = spriteArray[i+2];
        gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[3] = spriteArray[i+3];
    }

    private void centerPosition()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        transform.position = position;
    }

    private void timedDetonation(float timer)
    {
        Invoke("bombExplode", timer);
    }

    public void bombExplode()
    {
        Vector2 bombPosition =  gameObject.transform.position;
        bombPosition.x = Mathf.Round(bombPosition.x);
        bombPosition.y = Mathf.Round(bombPosition.y);

        FindObjectOfType<GameManager>().destroyBomb(this.gameObject);

        Explosion explosionStart = Instantiate(explosionPrefab, bombPosition, Quaternion.identity);
        explosionStart.setActiveRenderer(explosionStart.spriteRendererStart);
        explosionStart.destroyAfter(explosionDuration);

        Vector2[] directions = {Vector2.up, Vector2.down, Vector2.right, Vector2.left};
        foreach (Vector2 direction in directions) {
            Vector2 explosionPosition = bombPosition;
            for (int i=1; i <= explosionLength; i++) {
                Collider2D collider;
                explosionPosition += direction;

                if (collider = Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("Bomb"))) {
                    // explosion hit a bomb -> trigger bomb
                    collider.gameObject.GetComponent<Bomb>().timedDetonation(0.1f);
                    if (this.type != BombType.PierceBomb) break;

                } else if (Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("SoftBlock"))) {
                    // explosion hit a soft wall -> trigger wall destruction animation
                    clearDestructible(explosionPosition);
                    if (this.type != BombType.PierceBomb) break;

                } else if (Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("HardBlock"))) {
                    // explosion hit a hard wall -> stop
                    break;

                } else if (Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("Explosion"))) {
                    // explosion hit another explosion -> stop
                    if (this.type != BombType.PierceBomb) break;

                } else if (collider = Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("Item"))) {
                    // explosion hit an item -> explode item
                    collider.gameObject.GetComponent<Item>().itemExplode();
                    if (this.type != BombType.PierceBomb) break;

                } else {
                    // empty square -> explode it
                    Explosion explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
                    explosion.setActiveRenderer(i == explosionLength ? explosion.spriteRendererEnd : explosion.spriteRendererMiddle);
                    explosion.setDirection(direction);
                    explosion.destroyAfter(explosionDuration);
                }
            }
        }
    }

    private void clearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTilemap.WorldToCell(position);
        TileBase tile = destructibleTilemap.GetTile(cell);
        if (tile != null) {
            Instantiate(SoftBlockPrefab, position, Quaternion.identity);
            destructibleTilemap.SetTile(cell, null);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        // print("bomb OnCollisionEnter2D with " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Player")) {
            PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();
            PlayerMovementController playerMovement = other.gameObject.GetComponent<PlayerMovementController>();
            if (player.kick) {
                this.direction = playerMovement.direction;
            } else {
                direction = Vector2.zero;
                centerPosition();
            }
        } else if (other.gameObject.CompareTag("Bomb") || other.gameObject.CompareTag("Stage")) {
            direction = Vector2.zero;
            centerPosition();
        }
    }

}
