using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceNodeDropItem
{
    public GameObject asssociatedGameObject;
    public float percentageChance;
}

[System.Serializable]
public class ResourceNodeDropTable
{
    public List<ResourceNodeDropItem> thisNodeDropItems = new List<ResourceNodeDropItem>();
}

public enum NodeGatherType
{
    Destroy_On_Gather,
    Continuous_Gather
}

public class ResourceNode : MonoBehaviour {

    //public RunnerResource requiredToHarvest;
    public EquipmentType equipmentTypeToHarvest;
    public int equipmentLevelToHarvest;

    //This needs to be a table with chances or something. But for now will just spawn this x 2
    //public GameObject droppedItemPickup;
    public ResourceNodeDropTable dropTable;

    public bool doesRespawn = true;

    public GameObject meshObject;

    Transform colliderRef;

    //ResourcesManager resourcesMan;
    EquipmentManager equipMan;

    public NodeGatherType gatherType;

    public float gatherTick = 0f;
    float lastGatherTime = 0f;

    void Start()
    {
        equipMan = GameObject.Find("ResourcesManager").GetComponent<EquipmentManager>();
    }

    void Update()
    {
        if (lastGatherTime > 0f)
        {
            lastGatherTime -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            //If player has required item
            if (equipMan.HasRequiredEquipment(equipmentTypeToHarvest, equipmentLevelToHarvest))
            {
                switch(gatherType)
                {
                    case NodeGatherType.Destroy_On_Gather:
                        //Drop items
                        colliderRef = col.transform;
                        DropItems_DestroyGather();
                        DeactivateNode();
                        break;
                    case NodeGatherType.Continuous_Gather:
                        if (lastGatherTime <= 0f)
                        {
                            colliderRef = col.transform;
                            DropItems_ContinuousGather();
                        }
                        break;
                }
            }
            else
            {
                Debug.Log("Player does not have item required to harvest");
                //Game may bump player, or give them damage? Though probably node dependent.
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerLayer"))
        {
            //If player has required item
            if (equipMan.HasRequiredEquipment(equipmentTypeToHarvest, equipmentLevelToHarvest))
            {
                switch (gatherType)
                {
                    case NodeGatherType.Continuous_Gather:
                        if (lastGatherTime <= 0f)
                        {
                            colliderRef = col.transform;
                            DropItems_ContinuousGather();
                        }
                        break;
                }
            }
            else
            {
                Debug.Log("Player does not have item required to harvest");
                //Game may bump player, or give them damage? Though probably node dependent.
            }
        }

    }

    void DropItems_DestroyGather()
    {
        for(int i = 0; i < dropTable.thisNodeDropItems.Count; i++)
        {
            float randomVal = Random.Range(0f, 1f);
            if(randomVal <= dropTable.thisNodeDropItems[i].percentageChance)
            {
                GameObject g = Instantiate(dropTable.thisNodeDropItems[i].asssociatedGameObject, transform);
                g.GetComponent<ItemPickup>().playerTransformRef = colliderRef;
                g.GetComponent<ItemPickup>().ThrowForward();
            }
        }
    }

    void DropItems_ContinuousGather()
    {
        for (int i = 0; i < dropTable.thisNodeDropItems.Count; i++)
        {
            float randomVal = Random.Range(0f, 1f);
            if (randomVal <= dropTable.thisNodeDropItems[i].percentageChance)
            {
                GameObject g = Instantiate(dropTable.thisNodeDropItems[i].asssociatedGameObject, transform);
                g.GetComponent<ItemPickup>().playerTransformRef = colliderRef;
                g.GetComponent<ItemPickup>().ThrowForward();
            }
        }

        lastGatherTime = gatherTick;
    }

    public void ActivateNode()
    {
        if (doesRespawn)
        {
            gameObject.SetActive(true);
        }
    }

    public void DeactivateNode()
    {
        meshObject.SetActive(false);
    }
}
