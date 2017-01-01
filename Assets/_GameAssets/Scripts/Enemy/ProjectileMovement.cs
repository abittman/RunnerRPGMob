using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {

    public PlayerStatus pStatus;
    public float moveSpeed = 10f;

    float projectileDamageAmount = 10f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
	}

    void OnTriggerEnter(Collider col)
    {
        pStatus.DamageHealth(projectileDamageAmount);

        Destroy(gameObject);
    }
}
