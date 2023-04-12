using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Bomb, Fire, Skate, RedBomb, FullFire, BombPass, Skull, Vest, Heart,
        RemoteControl, WallPass, Kick, PowerGlove, PowerBomb, Clock, Geta,
    }

    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        player.GetComponent<PlayerItemController>().OnItemPickup(this);
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
