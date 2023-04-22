using UnityEngine;

// controls bomb explosion animation in four directions
public class Explosion : MonoBehaviour
{
    public AnimatedSpriteRenderer spriteRendererStart;
    public AnimatedSpriteRenderer spriteRendererMiddle;
    public AnimatedSpriteRenderer spriteRendererEnd;

    // these are passed from the bomb at detonation time
    private AnimatedSpriteRenderer activeSpriteRenderer;
    private float explosionDuration;
    private ColourType colour;

    public void init(ColourType colour, AnimatedSpriteRenderer activeSpriteRenderer, float explosionDuration)
    {
        this.colour = colour;
        this.explosionDuration = explosionDuration;
        setActiveRenderer(activeSpriteRenderer);
        setSprites();
    }

    public void init(ColourType colour, AnimatedSpriteRenderer activeSpriteRenderer, float explosionDuration, Vector2 direction)
    {
        init(colour, activeSpriteRenderer, explosionDuration);
        setDirection(direction);
    }

    private void Start()
    {
        Destroy(gameObject, explosionDuration);
    }

    // select explosion sprites according to bomb colour
    private void setSprites()
    {
        Sprite[] spriteArray = Resources.LoadAll<Sprite>("Bombs/" + System.Enum.GetName(typeof(ColourType), colour));
        int i;
        AnimatedSpriteRenderer asr;
        if (activeSpriteRenderer == spriteRendererStart) {
            i = 14;
            asr = gameObject.transform.GetChild(0).gameObject.GetComponent<AnimatedSpriteRenderer>();
        } else if (activeSpriteRenderer == spriteRendererMiddle) {
            i = 13;
            asr = gameObject.transform.GetChild(1).gameObject.GetComponent<AnimatedSpriteRenderer>();
        } else {
            i = 11;
            asr = gameObject.transform.GetChild(2).gameObject.GetComponent<AnimatedSpriteRenderer>();
        }
        asr.spriteRenderer.sprite = asr.idleSprite = spriteArray[i];
        for(int j=0; j<5; j++) {
            asr.animationSprites[j]   = spriteArray[i+j*8];
            asr.animationSprites[9-j] = spriteArray[i+j*8];
        }
    }

    private void setActiveRenderer(AnimatedSpriteRenderer renderer)
    {
        activeSpriteRenderer = renderer;
        spriteRendererStart.enabled  = (renderer == spriteRendererStart);
        spriteRendererMiddle.enabled = (renderer == spriteRendererMiddle);
        spriteRendererEnd.enabled    = (renderer == spriteRendererEnd);
    }

    private void setDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        transform.rotation = Quaternion.AngleAxis(angle*Mathf.Rad2Deg, Vector3.forward);
    }
}
