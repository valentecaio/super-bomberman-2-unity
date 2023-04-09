using UnityEngine;

// controls destructible animation and item apparition
public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f;

    [Range(0f, 1f)]
    public float itemSpawnChance = 0.2f;

    public GameObject[] itemPrefabs;

    public void Start()
    {
        Destroy(this.gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        if (itemPrefabs.Length > 0 && Random.value < itemSpawnChance) {
            int randIndex = Random.Range(0, itemPrefabs.Length);
            Instantiate(itemPrefabs[randIndex], transform.position, Quaternion.identity);
        }
    }
}
