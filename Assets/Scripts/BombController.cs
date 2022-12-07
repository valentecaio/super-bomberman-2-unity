using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Bomb")]
    public KeyCode inputKey = KeyCode.Space;
    public GameObject bombPrefab;
    public float bombTimer = 3f;
    private int bombsRemaining;
    public int bombAmount = 1;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionLength = 2;

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
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.setActiveRenderer(length == 1 ? explosion.spriteRendererEnd : explosion.spriteRendererMiddle);
        explosion.setDirection(direction);
        explosion.destroyAfter(explosionDuration);
        explode(position, direction, length-1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bomb")) {
            other.isTrigger = false;
        }
    }
}
