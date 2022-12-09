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
            int bombAmount = player.GetComponent<BombController>().bombAmount;
            player.GetComponent<BombController>().setBombAmount(bombAmount+1);
        } else if (type == ItemType.Fire) {
            player.GetComponent<BombController>().explosionLength++;
        } else if (type == ItemType.Speed) {
            player.GetComponent<MovementController>().speed++;
        }
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            OnItemPickup(other.gameObject);
        }
    }
}
