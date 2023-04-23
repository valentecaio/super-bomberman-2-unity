using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBombController : MonoBehaviour
{
    public KeyCode deployBombKey = KeyCode.Space;
    public KeyCode detonateBombKey = KeyCode.LeftAlt;
    public Tilemap destructibleTilemap;
    public GameObject commonBombPrefab;
    public GameObject remoteControlBombPrefab;
    public GameObject powerBombPrefab;
    public GameObject pierceBombPrefab;

    private PlayerStatus player;

    private void Start()
    {
        player = gameObject.GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(deployBombKey) && player.bombs.Count < player.bombAmount) {
            placeBomb();
        } else if (Input.GetKeyDown(detonateBombKey)) {
            foreach (GameObject bomb in player.bombs) {
                if (bomb.GetComponent<Bomb>().type == BombType.RemoteControl) {
                    bomb.GetComponent<Bomb>().bombExplode();
                    break;
                }
            }
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

        GameObject bomb = Instantiate(commonBombPrefab, position, Quaternion.identity);
        bomb.GetComponent<Bomb>().init(player, destructibleTilemap);
        player.bombs.Add(bomb);

        // we ignore this collision until the player exits the bomb sprite
        player.droppingBomb = true;
        Physics2D.IgnoreCollision(bomb.GetComponent<CircleCollider2D>(), gameObject.GetComponent<CircleCollider2D>());
    }
}
