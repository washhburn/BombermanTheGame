using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;

    public void OnPlayButton()
    {
        SceneManager.LoadScene("BombermanTheGame");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}