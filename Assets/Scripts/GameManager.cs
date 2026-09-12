using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void WinGame()
    {
        SceneManager.LoadScene("main menu");
    }
}