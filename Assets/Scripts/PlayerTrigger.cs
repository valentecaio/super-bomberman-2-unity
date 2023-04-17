using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bomb")) {
            PlayerStatus player = gameObject.GetComponentInParent<PlayerStatus>();
            player.droppingBomb = false;
        }
    }
}
