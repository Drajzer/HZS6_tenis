using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePause : MonoBehaviour
{
    private bool paused;
    [SerializeField]
    private GameObject blur;
    [SerializeField]
    private GameObject GameplayStuff;
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private GameObject Menu1;
    [SerializeField]
    private ManageGame gameManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameManager.canPause)
        {
            paused = !paused;
            if (paused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    void Pause()
    {
        blur.SetActive(true);
        Time.timeScale = 0;
        AudioListener.pause = true;
        GameplayStuff.SetActive(false);
        Menu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void UnPause()
    {
        blur.SetActive(false);
        Time.timeScale = 1;
        AudioListener.pause = false;
        GameplayStuff.SetActive(true);
        Menu.SetActive(false);
        Menu1.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
