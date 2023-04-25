using UnityEngine;

// this trigger is necessary so the player can ignore collisions with new bombs
// but still collide with them after moving around
public class PlayerTrigger : MonoBehaviour
{
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bomb")) {
            Player player = gameObject.GetComponentInParent<Player>();
            player.droppingBomb = false;
        }
    }
}
