using UnityEngine;

// controlls bomb explosion animation in four directions
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
}
