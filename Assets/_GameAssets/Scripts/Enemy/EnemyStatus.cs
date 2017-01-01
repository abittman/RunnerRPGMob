using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour {

    public EnemyMovement eMover;
    public EnemyCombat eCombat;

    public float currentHealth;
    public float maxHealth;

    public Image healthBarImage;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        healthBarImage.fillAmount = currentHealth / maxHealth;
	}

    public void DamageEnemy(float damage)
    {
        currentHealth -= damage;
        healthBarImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0f)
        {
            EnemyDies();
        }
    }

    void EnemyDies()
    {
        eCombat.EnemyOutOfCombat();
        gameObject.SetActive(false);
    }

    public void ResetEnemyStatus()
    {
        currentHealth = maxHealth;
    }
}
