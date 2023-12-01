using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManger : MonoBehaviour
{
    public void RelaoadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene(3);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
