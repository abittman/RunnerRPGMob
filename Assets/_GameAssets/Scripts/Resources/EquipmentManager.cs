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

    public PathPoolManager ppMan;

    void Awake()
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
    public void ObtainedNewEquipment(string equipmentID)
    {
        //Check axe. If axe, add equipment
        EquipmentItem localRef = axeEquipments.Find(x => x.equipmentName == equipmentID);
        if (localRef != null)
        {
            localRef.isObtained = true;

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

            ppMan.currentPathPoolGroup.Fixed_SetupLocalReferences();
        }
        else
        {
            Debug.LogError("Equipment of name " + equipmentID + " does not exist");
        }
    }
}
