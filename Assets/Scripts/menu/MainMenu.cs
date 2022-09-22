using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
   {
        SceneManager.LoadScene("PersistentScene");
   }

   public void quitGame()
   {
     Application.Quit();

   }

    public void web3()
    {
        SceneManager.LoadScene("Web3Panel");
    }

    public void backToMainMenu()
    {
        SceneManager.LoadScene("PersistentScene");
    }
}
