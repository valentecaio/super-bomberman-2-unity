using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int bombAmount = 2;
    public int fireAmout = 2;
    public float speed = 4f;
    public bool bombPass = false;
    public bool wallPass = false;
    public bool kick = false;
    public BombType bombType = BombType.Common;
}
