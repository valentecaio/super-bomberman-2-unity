using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType type;
    public AnimatedSpriteRenderer SpriteRendererExplosion;

    [HideInInspector]
    public bool exploding = false;
    private float destructionTime = 0.85f;

    public void itemExplode()
    {
        exploding = true;
        gameObject.GetComponent<AnimatedSpriteRenderer>().enabled = false;
        SpriteRendererExplosion.animationTime = destructionTime / SpriteRendererExplosion.animationSprites.Length;
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
