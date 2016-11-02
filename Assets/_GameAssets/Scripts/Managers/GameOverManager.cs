using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

    public GameObject gameOverUI;

    void Awake()
    {
        gameOverUI.SetActive(false);
    }

	public void DoGameOver()
    {
        gameOverUI.SetActive(true);
    }
}
