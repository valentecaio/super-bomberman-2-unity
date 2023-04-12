using UnityEngine;
using System.Collections.Generic;

public class PlayerItemController : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private PlayerStatus player;

    private void Start()
    {
        player = gameObject.GetComponent<PlayerStatus>();
    }

    public void OnItemPickup(Item item)
    {
        items.Add(item);
        PlayerStatus player = gameObject.GetComponent<PlayerStatus>();

        switch(item.type) {
            case Item.ItemType.Bomb:
                if (player.bombAmount < 8) {
                    player.bombAmount++;
                }
                break;

            case Item.ItemType.Fire:
                if (player.fireAmout < 8) {
                    player.fireAmout++;
                }
                break;

            case Item.ItemType.RedBomb:
                player.bombType = BombType.RedBomb;
                break;

            case Item.ItemType.FullFire:
                player.fireAmout = 8;
                break;

            case Item.ItemType.BombPass:
                break;

            case Item.ItemType.Skull:
                break;

            case Item.ItemType.Vest:
                break;

            case Item.ItemType.RemoteControl:
                player.bombType = BombType.RemoteControl;
                break;

            case Item.ItemType.WallPass:
                break;

            case Item.ItemType.Skate:
                if (player.speed < 8) {
                    player.speed++;
                }
                break;

            case Item.ItemType.Kick:
                break;

            case Item.ItemType.PowerGlove:
                break;

            case Item.ItemType.PowerBomb:
                player.bombType = BombType.PowerBomb;
                break;

            case Item.ItemType.Clock:
                break;

            case Item.ItemType.Heart:
                break;

            case Item.ItemType.Geta:
                break;
        }
    }
}
