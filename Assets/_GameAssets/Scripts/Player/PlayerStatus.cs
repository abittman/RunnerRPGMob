using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

    [Header("References")]
    public PlayerRunner pRunner;
    public PlayerSwipeInput pInput;
    public GameOverManager gameOverMan;

    public int maxHealth = 100;
    public int currentHealth = 100;

	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
	}

    //For when the player hits an obstacle or falls
    public void PlayerRunFails()
    {
        pRunner.StopRunner();
        pInput.canControl = false;
        gameOverMan.DoGameOver();
    }

    //For when the player dies from loss of health
    public void DoPlayerDeath()
    {

    }

    public void DamageHealth(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            DoPlayerDeath();
        }
    }

    public void FullyHealPlayer()
    {
        currentHealth = maxHealth;
    }

    public void IncreaseMaxHealth(int increaseAmount)
    {
        maxHealth += increaseAmount;
        //LATER - Save here
    }
}
