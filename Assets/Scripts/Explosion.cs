using UnityEngine;

// controls bomb explosion animation in four directions
public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRenderer spriteRendererStart;
    public AnimatedSpriteRenderer spriteRendererMiddle;
    public AnimatedSpriteRenderer spriteRendererEnd;
    private AnimatedSpriteRenderer activeSpriteRenderer;

    public void setActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        spriteRendererStart.enabled  = (renderer == spriteRendererStart);
        spriteRendererMiddle.enabled = (renderer == spriteRendererMiddle);
        spriteRendererEnd.enabled    = (renderer == spriteRendererEnd);
    }

    public void setDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle*Mathf.Rad2Deg, Vector3.forward);
    }
    
    public void destroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // print("explosion OnTriggerEnter2D with tag " + other.gameObject.tag);
        if (other.gameObject.tag == "Player") {
            PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();
            if (player.heart) {
                player.heart = false;
            } else {
                StartCoroutine(other.gameObject.GetComponent<PlayerMovementController>().die());
            }
        }
    }
}
