using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBombController : MonoBehaviour
{
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public Tilemap destructibleTilemap;

    private PlayerStatus player;

    private void Start()
    {
        player = gameObject.GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(inputKey) && player.bombs.Count < player.bombAmount) {
            placeBomb();
        }
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
        bomb.GetComponent<Bomb>().destructibleTilemap = destructibleTilemap;
        bomb.GetComponent<Bomb>().explosionLength = player.fireAmout;
        player.bombs.Add(bomb);

        // we ignore this collision until the player exits the bomb sprite
        player.droppingBomb = true;
        Physics2D.IgnoreCollision(bomb.GetComponent<CircleCollider2D>(), gameObject.GetComponent<CircleCollider2D>());

        StartCoroutine(bomb.GetComponent<Bomb>().run());
    }
}
