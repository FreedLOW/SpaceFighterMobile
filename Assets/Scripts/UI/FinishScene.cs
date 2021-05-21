using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScene : MonoBehaviour
{
    public void MenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}