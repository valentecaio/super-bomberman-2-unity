using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;
    public Dictionary<ColourType, Sprite[]> spritesBomber = new Dictionary<ColourType, Sprite[]>();
    public Dictionary<ColourType, Sprite[]> spritesBomb = new Dictionary<ColourType, Sprite[]>();

    public void Awake()
    {
        // ignore collision between players
        foreach (GameObject p1 in players) {
            foreach (GameObject p2 in players) {
                Physics2D.IgnoreCollision(p1.GetComponent<CircleCollider2D>(), p2.GetComponent<CircleCollider2D>());
            }
        }

        // load bomberman sprites
        foreach (string colour in Enum.GetNames(typeof(ColourType))) {
            Sprite[] spriteArray = Resources.LoadAll<Sprite>("Bombers/" + colour);
            spritesBomber.Add(Enum.Parse<ColourType>(colour), spriteArray);
        }

        // load bomb sprites
        foreach (string colour in Enum.GetNames(typeof(ColourType))) {
            if (colour == "Infected") continue; // skip infected colour
            Sprite[] spriteArray = Resources.LoadAll<Sprite>("Bombs/" + colour);
            spritesBomb.Add(Enum.Parse<ColourType>(colour), spriteArray);
        }
    }

    public void destroyBomb(GameObject bomb)
    {
        foreach (GameObject p in players) {
            p.GetComponent<Player>().bombs.Remove(bomb);
        }
        Destroy(bomb);
    }
 
    public void checkWinState()
    {
        int aliveCount = 0;
        foreach (GameObject p in players) {
            if (p.activeSelf) {
                aliveCount++;
            }
        }

        if (aliveCount <= 1) {
            Invoke(nameof(newRound), 3f);
        }
    }

    private void newRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
