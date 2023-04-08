using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombTimer = 3f;
    public int bombAmount = 2;
    public List<GameObject> bombs = new List<GameObject>();

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerStage;
    public LayerMask explosionLayerBomb;
    public LayerMask explosionLayerExplosion;
    public LayerMask explosionLayerItem;
    public float explosionDuration = 0.85f;
    public int explosionLength = 2;

    [Header("Destructible")]
    public Destructible destructiblePrefab;
    public Tilemap destructibleTilemap;

    private void Update()
    {
        if (Input.GetKeyDown(inputKey) && bombs.Count < bombAmount) {
            StartCoroutine(placeBomb());
        }
    }

    private IEnumerator placeBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
        bombs.Add(bomb);

        // works like a sleep(3)
        yield return new WaitForSeconds(bombTimer);

        // the bomb may have been destroyed by another bomb
        if (bomb) {
            bombExplode(bomb);
        }
    }

    private void bombExplode(GameObject bomb)
    {
        Vector2 position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        FindObjectOfType<GameManager>().destroyBomb(bomb);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.setActiveRenderer(explosion.spriteRendererStart);
        explosion.destroyAfter(explosionDuration);

        recursiveExplode(position, Vector2.up, explosionLength);
        recursiveExplode(position, Vector2.down, explosionLength);
        recursiveExplode(position, Vector2.left, explosionLength);
        recursiveExplode(position, Vector2.right, explosionLength);
    }

    private void recursiveExplode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0) return;

        position += direction;
        Collider2D collider;

        if (collider = Physics2D.OverlapBox(position, Vector2.one/2f, 0f, explosionLayerBomb)) {
            // explosion hit a bomb -> trigger bomb
            bombExplode(collider.gameObject);

        } else if (Physics2D.OverlapBox(position, Vector2.one/2f, 0f, explosionLayerExplosion)) {
            // necessary condition to avoid looping between bombs, just break recursion

        } else if (Physics2D.OverlapBox(position, Vector2.one/2f, 0f, explosionLayerStage)) {
            // explosion hit a wall -> trigger wall destruction animation
            clearDestructible(position);

        } else if (collider = Physics2D.OverlapBox(position, Vector2.one/2f, 0f, explosionLayerItem)) {
            // explosion hit an item -> delete item - TODO: animation
            Destroy(collider.gameObject);

        } else {
            // empty square -> instantiate explosion prefab and continue recursion
            Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.setActiveRenderer(length == 1 ? explosion.spriteRendererEnd : explosion.spriteRendererMiddle);
            explosion.setDirection(direction);
            explosion.destroyAfter(explosionDuration);
            recursiveExplode(position, direction, length-1);
        }
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

}
