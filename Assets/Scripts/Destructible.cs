using UnityEngine;

// controles destructible animation
public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f;

    public void Start()
    {
        Destroy(this.gameObject, destructionTime);
    }
}
