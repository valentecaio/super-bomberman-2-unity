using UnityEngine;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    public int bombAmount = 2;
    public int fireAmout = 2;
    public float speed = 4f;
    public bool heart = false;
    public bool kick = false;
    public bool bombPass = false;
    public bool wallPass = false;
    public BombType bombType = BombType.Common;

    public bool droppingBomb = false;

    private List<Item> items = new List<Item>();

    public void OnItemPickup(Item item)
    {
        items.Add(item);

        switch(item.type) {
            case Item.ItemType.Bomb:
                if (bombAmount < 8) {
                    bombAmount++;
                }
                break;

            case Item.ItemType.Fire:
                if (fireAmout < 8) {
                    fireAmout++;
                }
                break;

            case Item.ItemType.RedBomb:
                bombType = BombType.RedBomb;
                break;

            case Item.ItemType.FullFire:
                fireAmout = 8;
                break;

            case Item.ItemType.BombPass:
                bombPass = true;
                kick = false;
                break;

            case Item.ItemType.Skull:
                break;

            case Item.ItemType.Vest:
                break;

            case Item.ItemType.RemoteControl:
                bombType = BombType.RemoteControl;
                break;

            case Item.ItemType.WallPass:
                break;

            case Item.ItemType.Skate:
                if (speed < 8) {
                    speed++;
                }
                break;

            case Item.ItemType.Kick:
                bombPass = false;
                kick = true;
                break;

            case Item.ItemType.PowerGlove:
                break;

            case Item.ItemType.PowerBomb:
                bombType = BombType.PowerBomb;
                break;

            case Item.ItemType.Clock:
                break;

            case Item.ItemType.Heart:
                heart = true;
                break;

            case Item.ItemType.Geta:
                break;
        }
    }
}
