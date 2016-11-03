using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RunnerSceneManager : MonoBehaviour {

    public TownManagementManager townManagement;
    public PlayerRunner playerRunner;
    public PlayerSwipeInput pInput;

    public bool onMenu = true;
    public bool onGame = false;

    static RunnerSceneManager instance;

    void Start()
    {
        if(instance)
        {
            onMenu = instance.onMenu;
            onGame = instance.onGame;
            
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            //Default settings
            onMenu = true;
            onGame = false;
        }

        if (onMenu)
        {
            ToTownManagement();
        }
        else if(onGame)
        {
            ToRunner();
        }
    }

    public void RestartGame_ToMenu()
    {
        onGame = false;
        onMenu = true;
        //Restart game
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame_ToGame()
    {
        onMenu = false;
        onGame = true;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToTownManagement()
    {
        playerRunner.StopRunner();
        pInput.DeactivateControl();
        townManagement.ReturnToMainMenu();
    }

    public void ToRunner()
    {
        townManagement.CloseMainMenu();
        playerRunner.StartRunner();
        pInput.ActivateControl();
    }
}
