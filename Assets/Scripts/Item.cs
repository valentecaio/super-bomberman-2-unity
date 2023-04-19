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

    public bool exploding = false;
    private float destructionTime = 0.75f;

    public void itemExplode()
    {
        exploding = true;
        gameObject.GetComponent<AnimatedSpriteRenderer>().enabled = false;
        SpriteRendererExplosion.enabled = true;
        Destroy(this.gameObject, destructionTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // print("Item OnTriggerEnter2D tag = " + other.tag);
        if (other.CompareTag("Bomb")) {
            if (exploding) {
                other.GetComponent<Bomb>().bombExplode();
            } else {
                Destroy(this.gameObject);
            }
        }
    }
}
