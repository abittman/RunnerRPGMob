using UnityEngine;
using System.Collections;

public class WorldObstacles : MonoBehaviour {

    public PlayerStatus pStatus;

    public bool obstacleIsDestructible;

    public float obstacleHitDamage = 25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision cols)
    {
        if(cols.collider.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            pStatus.DamageHealth(obstacleHitDamage);

            if(obstacleIsDestructible)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
