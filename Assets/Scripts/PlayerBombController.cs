using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombTimer = 3f;
    public int bombAmount = 2;
    private int bombsRemaining;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 0.85f;
    public int explosionLength = 2;

    [Header("Destructible")]
    public Destructible destructiblePrefab;
    public Tilemap destructibleTilemap;

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey) && bombsRemaining > 0) {
            StartCoroutine(placeBomb());
        }
    }

    private IEnumerator placeBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombsRemaining--;

        // works like a sleep(3)
        yield return new WaitForSeconds(bombTimer);

        // time to explode
        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.setActiveRenderer(explosion.spriteRendererStart);
        explosion.destroyAfter(explosionDuration);

        explode(position, Vector2.up, explosionLength);
        explode(position, Vector2.down, explosionLength);
        explode(position, Vector2.left, explosionLength);
        explode(position, Vector2.right, explosionLength);

        Destroy(bomb);
        bombsRemaining++;
    }

    private void explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) {
            return;
        }

        position += direction;
        if (Physics2D.OverlapBox(position, Vector2.one/2f, 0f, explosionLayerMask)) {
            clearDestructible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.setActiveRenderer(length == 1 ? explosion.spriteRendererEnd : explosion.spriteRendererMiddle);
        explosion.setDirection(direction);
        explosion.destroyAfter(explosionDuration);
        explode(position, direction, length-1); // recursion
    }

    private void clearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTilemap.WorldToCell(position);
        TileBase tile = destructibleTilemap.GetTile(cell);
        if (tile != null) {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTilemap.SetTile(cell, null);
        }
    }

    // enable collision between bomb and players
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false;
        }
    }

    public void setBombAmount(int newBombAmount)
    {
        int bombsInGame = bombAmount - bombsRemaining;
        this.bombAmount = newBombAmount;
        this.bombsRemaining = newBombAmount - bombsInGame;
    }
}
