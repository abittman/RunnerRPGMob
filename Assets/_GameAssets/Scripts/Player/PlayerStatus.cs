using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

    [Header("References")]
    public PlayerRunner pRunner;
    public PlayerSwipeInput pInput;
    public GameOverManager gameOverMan;

    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Image healthbarImage;

	// Use this for initialization
	void Start ()
    {
        currentHealth = maxHealth;
        healthbarImage.fillAmount = currentHealth / maxHealth;
	}

    //For when the player hits an obstacle or falls
    public void PlayerRunFails()
    {
        pRunner.StopRunner();
        pInput.DeactivateControl();
        gameOverMan.DoGameOver();
    }

    //For when the player dies from loss of health
    public void DoPlayerDeath()
    {
        PlayerRunFails();
    }

    public void DamageHealth(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
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
