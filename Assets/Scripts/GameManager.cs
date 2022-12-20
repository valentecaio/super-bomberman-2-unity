using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;
 
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
