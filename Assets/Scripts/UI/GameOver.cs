using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Button Retry;
    public Button Exit;

    // Start is called before the first frame update
    void Start()
    {
        Retry.onClick.AddListener(RetryClicked);
        Exit.onClick.AddListener(ExitClicked);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void RetryClicked()
    {
        SceneManager.LoadScene("Scenes/Level_00");
    }
    
    void ExitClicked()
    {
        Application.Quit();
    }
}
