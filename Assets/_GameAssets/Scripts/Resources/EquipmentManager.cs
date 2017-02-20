using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    None,
    Wood_Cutting,
    Rock_Breaking,
    Crop_Harvesting,
    Ore_Mining,
    Fishing
}

[System.Serializable]
public class EquipmentItem
{
    public string equipmentName;
    public EquipmentType equipType;
    public int equipLevel;
    public bool isObtained = false;
}

public class EquipmentManager : MonoBehaviour {

    public List<EquipmentItem> axeEquipments = new List<EquipmentItem>();
    EquipmentItem currentAxe;

    public List<EquipmentItem> pickEquipments = new List<EquipmentItem>();
    EquipmentItem currentPick;

    void Start()
    {
        //Temp
        currentAxe = axeEquipments[0];
    }

    public bool HasRequiredEquipment(EquipmentType type, int level)
    {
        switch(type)
        {
            case EquipmentType.Wood_Cutting:
                Debug.Log(currentAxe.equipmentName + " " + currentAxe.equipLevel);
                if(currentAxe.equipLevel >= level)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case EquipmentType.Ore_Mining:
                if (currentPick.equipLevel >= level)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        //Catch all
        return false;
    }

    //[TODO] Link this to the ability to give new equipment (perhaps with reource manager)
    public void ObtainedNewEquipment(EquipmentItem newItem)
    {
        EquipmentItem localRef = null;
        switch(newItem.equipType)
        {
            case EquipmentType.Wood_Cutting:
                localRef = axeEquipments.Find(x => x.equipmentName == newItem.equipmentName);
                if (localRef != null)
                {
                    localRef.isObtained = true;
                }
                else
                {
                    axeEquipments.Add(newItem);
                }

                if (currentAxe == null)
                {
                    currentAxe = localRef;
                }
                else
                {
                    if (currentAxe.equipLevel < localRef.equipLevel)
                    {
                        currentAxe = localRef;
                    }
                }
                break;
            case EquipmentType.Ore_Mining:
                localRef = pickEquipments.Find(x => x.equipmentName == newItem.equipmentName);
                if (localRef != null)
                {
                    localRef.isObtained = true;
                }
                else
                {
                    pickEquipments.Add(newItem);
                }

                if (currentPick == null)
                {
                    currentPick = localRef;
                }
                else
                {
                    if(currentPick.equipLevel < localRef.equipLevel)
                    {
                        currentPick = localRef;
                    }
                }
                break;
        }
    }
}
