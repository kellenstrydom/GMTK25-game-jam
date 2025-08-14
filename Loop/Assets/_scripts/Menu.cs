using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string levelSceneName = "Level 1";

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
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}