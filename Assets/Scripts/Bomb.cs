using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float speed = 5f;
    public new Rigidbody2D rigidbody;

    private Vector2 direction = Vector2.zero;

    private void FixedUpdate()
    {
        Vector2 translation = direction * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + translation);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        // print("bomb OnCollisionEnter2D with " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Player")) {
            PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();
            PlayerMovementController playerMovement = other.gameObject.GetComponent<PlayerMovementController>();
            if (player.kick) {
                this.direction = playerMovement.direction;
            } else {
                direction = Vector2.zero;
                centerPosition();  
            }
        } else if (other.gameObject.CompareTag("Bomb") || other.gameObject.CompareTag("Stage")) {
            direction = Vector2.zero;
            centerPosition();
        }
    }

    private void centerPosition()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        transform.position = position;
    }
}
