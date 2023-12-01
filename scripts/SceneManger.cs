using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public void RelaoadCurrentScene()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(0);
    }
    public void LoadLevel1()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(1);
    }
    public void LoadLevel2()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(2);
    }
    public void LoadLevel3()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(3);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
