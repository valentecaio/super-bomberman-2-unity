using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject bombPrefab;
    public KeyCode inputKey = KeyCode.Space;
    public float bombTimer = 3f;
    public int bombAmount = 1;
    private int bombsRemaining;

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

        bombsRemaining++;
        Destroy(bomb);
    }
}
