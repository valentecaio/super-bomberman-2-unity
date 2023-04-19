using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Bomb, Fire, Skate, RedBomb, FullFire, BombPass, Skull, Vest, Heart,
        RemoteControl, WallPass, Kick, PowerGlove, PowerBomb, Clock, Geta,
    }

    public ItemType type;
    public AnimatedSpriteRenderer SpriteRendererExplosion;

    private float destructionTime = 0.75f;
    private bool exploding = false;

    public void itemExplode()
    {
        exploding = true;
        gameObject.GetComponent<AnimatedSpriteRenderer>().enabled = false;
        SpriteRendererExplosion.enabled = true;
        Destroy(this.gameObject, destructionTime);
    }

    private void OnItemPickup(GameObject player)
    {
        player.GetComponent<PlayerStatus>().OnItemPickup(this);
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // print("Item OnTriggerEnter2D tag = " + other.tag);
        if (exploding) {
            if (other.CompareTag("Player")) {
                StartCoroutine(other.gameObject.GetComponent<PlayerMovementController>().die());
            } else if (other.CompareTag("Bomb")) {
                other.GetComponent<Bomb>().bombExplode();
            }
        } else {
            if (other.CompareTag("Player")) {
                OnItemPickup(other.gameObject);
            } else if (other.CompareTag("Bomb")) {
                Destroy(this.gameObject);
            }
        }
    }
}
