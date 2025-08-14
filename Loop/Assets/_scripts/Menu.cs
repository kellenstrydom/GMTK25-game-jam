using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
  

    public GameObject disclaimer; 

    void Start()
    {
        
        if (PlayerPrefs.GetInt("VisitedStartBefore", 0) == 1)
        {
            
            if (disclaimer != null)
                disclaimer.SetActive(true);
        }
        else
        {
            if (disclaimer != null)
                disclaimer.SetActive(false);
        }
    }

    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene("Levels 1");
    }

    public void EndGame(string sceneName)
    {
        SceneManager.LoadScene("Home");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}