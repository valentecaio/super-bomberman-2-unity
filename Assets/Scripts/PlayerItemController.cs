using UnityEngine;
using System.Collections.Generic;

public class PlayerItemController : MonoBehaviour
{
    public List<ItemPickup> items = new List<ItemPickup>();

    private PlayerStatus player;

    private void Start()
    {
        player = gameObject.GetComponent<PlayerStatus>();
    }

    public void OnItemPickup(ItemPickup item)
    {
        items.Add(item);
        PlayerStatus player = gameObject.GetComponent<PlayerStatus>();

        switch(item.type) {
            case ItemPickup.ItemType.Bomb:
                if (player.bombAmount < 8) {
                    player.bombAmount++;
                }
                break;

            case ItemPickup.ItemType.Fire:
                if (player.fireAmout < 8) {
                    player.fireAmout++;
                }
                break;

            case ItemPickup.ItemType.RedBomb:
                player.bombType = BombType.RedBomb;
                break;

            case ItemPickup.ItemType.FullFire:
                break;

            case ItemPickup.ItemType.BombPass:
                break;

            case ItemPickup.ItemType.Skull:
                break;

            case ItemPickup.ItemType.Vest:
                break;

            case ItemPickup.ItemType.RemoteControl:
                player.bombType = BombType.RemoteControl;
                break;

            case ItemPickup.ItemType.WallPass:
                break;

            case ItemPickup.ItemType.Skate:
                if (player.speed < 8) {
                    player.speed++;
                }
                break;

            case ItemPickup.ItemType.Kick:
                break;

            case ItemPickup.ItemType.PowerGlove:
                break;

            case ItemPickup.ItemType.PowerBomb:
                player.bombType = BombType.PowerBomb;
                break;

            case ItemPickup.ItemType.Clock:
                break;

            case ItemPickup.ItemType.Heart:
                break;

            case ItemPickup.ItemType.Geta:
                break;
        }
    }
}
