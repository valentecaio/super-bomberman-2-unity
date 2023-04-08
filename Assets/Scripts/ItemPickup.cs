using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        Bomb, Fire, Speed
    }

    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        if (type == ItemType.Bomb) {
            player.GetComponent<PlayerBombController>().bombAmount++;
        } else if (type == ItemType.Fire) {
            player.GetComponent<PlayerBombController>().explosionLength++;
        } else if (type == ItemType.Speed) {
            player.GetComponent<PlayerMovementController>().speed++;
        }
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            OnItemPickup(other.gameObject);
        } else if (other.CompareTag("Bomb")) {
            Destroy(this.gameObject);
        }
    }
}
