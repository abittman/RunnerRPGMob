using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RunnerSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RestartGame()
    {
        //Restart game
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
