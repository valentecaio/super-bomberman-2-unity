using UnityEngine;

public class AnimationItemExploding : MonoBehaviour
{
    public float destructionTime = 0.75f;

    public void Start()
    {
        Destroy(this.gameObject, destructionTime);
    }
}
