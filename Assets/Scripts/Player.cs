using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [Header("Editor: Read & Write")]
    public int bombAmount = 2;
    public int fireAmout = 2;
    public float speed = 4f;
    public bool heart = false;
    public bool kick = false;
    public bool invincible = false;
    public BombType bombType = BombType.Common;
    public ColourType colour = ColourType.White;

    [Header("Editor: Read only")]
    public bool skull = false;
    public bool wallPass = false;
    public bool bombPass = false;

    [Header("Lists")]
    public List<GameObject> bombs = new List<GameObject>();
    public List<Item> items = new List<Item>();

    [Header("Constants & Logical State Parameters")]
    public bool skullSpriteActive = false;
    public float skullAnimationTime = 0.2f;
    public float invincibleTime = 2f;
    public int triggerEnterCount = 0;

    // should be called at player creation
    public void init(ColourType colour)
    {
        this.colour = colour;
        setSprites(colour);
    }

    private void Start()
    {
        setSprites(colour);
    }

    public void OnItemPickup(Item item)
    {
        items.Add(item);

        // picking a item removes the skull
        skull = false;
        items.RemoveAll(item => item.type == ItemType.Skull);

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
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bomb"), bombPass);
                break;

            case ItemType.Skull:
                skull = true;
                skullSpritesLoop();
                break;

            case ItemType.Vest:
                break;

            case ItemType.RemoteControl:
                bombType = BombType.RemoteControl;
                break;

            case ItemType.WallPass:
                wallPass = true;
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("SoftBlock"), wallPass);
                break;

            case ItemType.Skate:
                if (speed < 8) {
                    speed++;
                }
                break;

            case ItemType.Kick:
                bombPass = false;
                kick = true;
                Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Bomb"), bombPass);
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

    public void nextColour()
    {
        this.colour = (ColourType) ((((int) this.colour) +1) % 18);
        setSprites(colour);
    }

    private void unsetInvincible()
    {
        this.invincible = false;
    }

    private void tryToDie()
    {
        if (invincible) {
            return;
        }
        if (heart) {
            heart = false;
            invincible = true;
            Invoke("unsetInvincible", invincibleTime);
        } else {
            StartCoroutine(die());
        }
    }

    private IEnumerator die()
    {
        PlayerInputController pic = this.GetComponent<PlayerInputController>();
        this.enabled = false;
        pic.enabled = false;
        pic.spriteRendererUp.enabled = false;
        pic.spriteRendererDown.enabled = false;
        pic.spriteRendererLeft.enabled = false;
        pic.spriteRendererRight.enabled = false;
        pic.SpriteRendererDeath.enabled = true;
        yield return new WaitForSeconds(1.25f);
        pic.gameObject.SetActive(false);
        FindObjectOfType<GameManager>().checkWinState();
    }

    private void skullSpritesLoop()
    {
        if (skull || skullSpriteActive) {
            skullSpriteActive = !skullSpriteActive;
            setSprites(skullSpriteActive ? ColourType.Infected : this.colour);
            Invoke("skullSpritesLoop", skullAnimationTime);
        }
    }

    // select sprites according to player colour
    private void setSprites(ColourType colour)
    {
        Sprite[] spriteArray = FindObjectOfType<GameManager>().spritesBomber[colour];
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
        // calls to this function are duplicated becase the PlayerTrigger has isTrigger on
        // we ignore half of them
        triggerEnterCount++;
        if (triggerEnterCount%2 == 1)
            return;

        // print("OnTriggerEnter2D " + this.gameObject.tag + " " + other.gameObject.tag);
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
