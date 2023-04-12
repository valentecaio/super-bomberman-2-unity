using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// manages bomb life cycle
public class PlayerBombController : MonoBehaviour
{
    public enum BombType {
        Common, RedBomb, RemoteControl, PowerBomb
    }

    [Header("Prefab")]
    public GameObject bombPrefab;
    public Explosion explosionPrefab;
    public Destructible destructiblePrefab;

    [Header("Explosion")]
    public float explosionDuration = 0.85f;
    public Tilemap destructibleTilemap;

    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space;
    public float bombTimer = 3f;
    public List<GameObject> bombs = new List<GameObject>();

    private PlayerStatus player;

    private void Start()
    {
        player = gameObject.GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey) && bombs.Count < player.bombAmount) {
            StartCoroutine(placeBomb());
        }
    }

    private IEnumerator placeBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // we can't place a bomb over another one
        if (Physics2D.OverlapBox(position, Vector2.one/2f, 0f, LayerMask.GetMask("Bomb"))) {
            yield break;
        }

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
        Vector2 bombPosition = bomb.transform.position;
        bombPosition.x = Mathf.Round(bombPosition.x);
        bombPosition.y = Mathf.Round(bombPosition.y);

        FindObjectOfType<GameManager>().destroyBomb(bomb);

        Explosion explosionStart = Instantiate(explosionPrefab, bombPosition, Quaternion.identity);
        explosionStart.setActiveRenderer(explosionStart.spriteRendererStart);
        explosionStart.destroyAfter(explosionDuration);

        Vector2[] directions = {Vector2.up, Vector2.down, Vector2.right, Vector2.left};
        foreach (Vector2 direction in directions) {
            Vector2 explosionPosition = bombPosition;
            for (int i=1; i <= player.fireAmout; i++) {
                Collider2D collider;
                explosionPosition += direction;

                if (collider = Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("Bomb"))) {
                    // explosion hit a bomb -> trigger bomb
                    bombExplode(collider.gameObject);
                    break;

                } else if (Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("Stage"))) {
                    // explosion hit a wall -> trigger wall destruction animation
                    clearDestructible(explosionPosition);
                    break;

                } else if (collider = Physics2D.OverlapBox(explosionPosition, Vector2.one/2f, 0f, LayerMask.GetMask("Item"))) {
                    // explosion hit an item -> delete item - TODO: animation
                    Destroy(collider.gameObject);
                    break;

                } else {
                    // empty square -> explode it
                    Explosion explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
                    explosion.setActiveRenderer(i == player.fireAmout ? explosion.spriteRendererEnd : explosion.spriteRendererMiddle);
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


    public void setBombCollision(bool state)
    {
        // TODO
    }

}
