using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

    public ProgressManager progressMan;
    public GameObject gameOverUI;

    void Awake()
    {
        gameOverUI.SetActive(false);
    }

	public void DoGameOver()
    {
        progressMan.LoseProgress();
        gameOverUI.SetActive(true);
    }
}
