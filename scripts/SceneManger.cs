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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LoadMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void LoadLevel1()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(1);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LoadLevel2()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(2);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void LoadLevel3()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene(3);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
