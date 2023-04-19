using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    public float speed = 5f;
    public float bombTimer = 3f;
    public float explosionDuration = 0.75f;
    public Explosion explosionPrefab;
    public SoftBlock SoftBlockPrefab;

    // these are passed from the player on bomb instantiation
    public Tilemap destructibleTilemap;
    public int explosionLength = 2;

    private Vector2 direction = Vector2.zero;

    private void FixedUpdate()
    {
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().MovePosition(GetComponent<Rigidbody2D>().position + translation);
    }

    private void centerPosition()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        transform.position = position;
    }
    public void timedDetonation(float timer)
    {
        Invoke("bombExplode", timer);
    }

    public void startTimer()
    {
        timedDetonation(bombTimer);
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
                    break;

                } else if (Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("SoftBlock", "HardBlock"))) {
                    // explosion hit a wall -> trigger wall destruction animation
                    clearDestructible(explosionPosition);
                    break;

                } else if (Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("Explosion"))) {
                    // explosion hit another explosion -> stop
                    break;

                } else if (collider = Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("Item"))) {
                    // explosion hit an item -> explode item
                    collider.gameObject.GetComponent<Item>().itemExplode();
                    break;

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
