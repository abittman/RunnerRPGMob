using UnityEngine;
using System.Collections;

public class ItemPickup : MonoBehaviour {

    public RunnerResource resourceVal;
    public ResourcesManager resourcesMan;
    //public TownSectionSpawn parentSection;
    public Transform playerTransformRef;

    public Rigidbody thisRB;
    public float forceVal = 500f;

    public bool canMoveToPlayer = false;

    Vector3 startPos;
    float moveToTimer = 0f;

    // Use this for initialization
    void Start () {
        resourcesMan = GameObject.Find("ResourcesManager").GetComponent<ResourcesManager>();
    }

    public void ThrowForward()
    {
        //Start pos
        transform.position = playerTransformRef.position + playerTransformRef.forward;
        Vector3 throwVector = playerTransformRef.forward;
        throwVector.y = 0.5f;
        Debug.DrawLine(transform.position, transform.position + throwVector, Color.red, 2f);
        thisRB.AddForce(forceVal * throwVector);
        StartCoroutine(WaitToMoveToPlayer());
    }
	
    IEnumerator WaitToMoveToPlayer()
    {
        yield return new WaitForSeconds(2f);

        canMoveToPlayer = true;
        startPos = transform.position;
    }

    void Update()
    {
        if(canMoveToPlayer)
        {
            MoveTowardPlayer();
        }
    }

    void MoveTowardPlayer()
    {
        moveToTimer += Time.deltaTime;
        //Move to player
        transform.position = Vector3.Lerp(startPos, playerTransformRef.position, moveToTimer);

        //When reached, deactivate item
        if(moveToTimer >= 1f)
        {
            DeactivatePickup();
        }
    }

    void DeactivatePickup()
    {
        resourcesMan.AddResource(resourceVal, false);
        gameObject.SetActive(false);
    }
}
