using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void ChangewhiteButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ChangeblackButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");
    }

}
