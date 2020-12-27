using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject activists;
    public void Quit()
    {
        //nao funciona enquanto Testamos no unity
        Application.Quit();
    }

    public void Reset()
    {
        ActivistsManager activistsManager = activists.GetComponent<ActivistsManager>();
        activistsManager.currentPlayer.value = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
