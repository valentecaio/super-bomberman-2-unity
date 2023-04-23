using UnityEngine;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    public int bombAmount = 2;
    public int fireAmout = 2;
    public float speed = 4f;
    public bool heart = false;
    public bool kick = false;
    public BombType bombType = BombType.Common;
    public ColourType colour = ColourType.White;

    public List<GameObject> bombs = new List<GameObject>();
    public List<Item> items = new List<Item>();

    private bool _wallPass = false;
    private bool _bombPass = false;
    private bool _droppingBomb = false;

    public bool wallPass {
        get { return _wallPass; }
        set {
            _wallPass = value;
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SoftBlock"), value);
        }
    }

    public bool bombPass {
        get { return _bombPass; }
        set {
            _bombPass = value;
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bomb"), value);
        }
    }

    public bool droppingBomb {
        get { return _droppingBomb; }
        set {
            _droppingBomb = value;
            if (!_droppingBomb) {
                foreach (GameObject bomb in bombs) {
                    Physics2D.IgnoreCollision(bomb.GetComponent<CircleCollider2D>(), gameObject.GetComponent<CircleCollider2D>(), false);
                }
            }
        }
    }


    // must be called at player creation
    public void init(ColourType colour)
    {
        this.colour = colour;
        setSprites();
    }

    private void Start()
    {
        setSprites();
    }

    public void OnItemPickup(Item item)
    {
        items.Add(item);

        switch(item.type) {
            case ItemType.Bomb:
                if (bombAmount < 8) {
                    bombAmount++;
                }
                break;

            case ItemType.Fire:
                if (fireAmout < 8) {
                    fireAmout++;
                }
                break;

            case ItemType.RedBomb:
                bombType = BombType.PierceBomb;
                break;

            case ItemType.FullFire:
                fireAmout = 8;
                break;

            case ItemType.BombPass:
                bombPass = true;
                kick = false;
                break;

            case ItemType.Skull:
                break;

            case ItemType.Vest:
                break;

            case ItemType.RemoteControl:
                bombType = BombType.RemoteControl;
                break;

            case ItemType.WallPass:
                wallPass = true;
                break;

            case ItemType.Skate:
                if (speed < 8) {
                    speed++;
                }
                break;

            case ItemType.Kick:
                bombPass = false;
                kick = true;
                break;

            case ItemType.PowerGlove:
                break;

            case ItemType.PowerBomb:
                bombType = BombType.PowerBomb;
                break;

            case ItemType.Clock:
                break;

            case ItemType.Heart:
                heart = true;
                break;

            case ItemType.Geta:
                break;
        }
    }

    public bool hasAPowerBombDeployed()
    {
        foreach (GameObject bomb in bombs)
            if (bomb.GetComponent<Bomb>().type == BombType.PowerBomb)
                return true;
        return false;
    }

    private void tryToDie()
    {
        if (heart) {
            heart = false;
        } else {
            StartCoroutine(gameObject.GetComponent<PlayerMovementController>().die());
        }
    }

    // select sprites according to player colour
    private void setSprites()
    {
        Sprite[] spriteArray = Resources.LoadAll<Sprite>("Bombers/" + System.Enum.GetName(typeof(ColourType), colour));
        // up
        gameObject.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[0];
        gameObject.transform.GetChild(1).gameObject.GetComponent<AnimatedSpriteRenderer>().idleSprite = spriteArray[0];
        gameObject.transform.GetChild(1).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[0] = spriteArray[0];
        gameObject.transform.GetChild(1).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[1] = spriteArray[1];
        gameObject.transform.GetChild(1).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[2] = spriteArray[0];
        gameObject.transform.GetChild(1).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[3] = spriteArray[2];
        // down
        gameObject.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[0];
        gameObject.transform.GetChild(2).gameObject.GetComponent<AnimatedSpriteRenderer>().idleSprite = spriteArray[6];
        gameObject.transform.GetChild(2).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[0] = spriteArray[6];
        gameObject.transform.GetChild(2).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[1] = spriteArray[7];
        gameObject.transform.GetChild(2).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[2] = spriteArray[6];
        gameObject.transform.GetChild(2).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[3] = spriteArray[8];
        // left
        gameObject.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[0];
        gameObject.transform.GetChild(3).gameObject.GetComponent<AnimatedSpriteRenderer>().idleSprite = spriteArray[9];
        gameObject.transform.GetChild(3).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[0] = spriteArray[9];
        gameObject.transform.GetChild(3).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[1] = spriteArray[10];
        gameObject.transform.GetChild(3).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[2] = spriteArray[9];
        gameObject.transform.GetChild(3).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[3] = spriteArray[11];
        // right
        gameObject.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[0];
        gameObject.transform.GetChild(4).gameObject.GetComponent<AnimatedSpriteRenderer>().idleSprite = spriteArray[3];
        gameObject.transform.GetChild(4).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[0] = spriteArray[3];
        gameObject.transform.GetChild(4).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[1] = spriteArray[4];
        gameObject.transform.GetChild(4).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[2] = spriteArray[3];
        gameObject.transform.GetChild(4).gameObject.GetComponent<AnimatedSpriteRenderer>().animationSprites[3] = spriteArray[5];
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // print("playerStatus OnTriggerEnter2D with tag " + other.gameObject.tag);
        if (other.gameObject.tag == "Explosion") {
            tryToDie();
        } else if (other.gameObject.CompareTag("Item")) {
            Item item = other.gameObject.GetComponent<Item>();
            if (item.exploding) {
                tryToDie();
            } else {
                OnItemPickup(item);
                Destroy(other.gameObject);
            }
        }
    }

}
