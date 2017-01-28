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

public enum EquipmentStatus
{
    None,
    Past,
    Current,
    Next,
    Future
}

[System.Serializable]
public class EquipmentItem
{
    public string equipmentName;
    public EquipmentType equipType;
    public int equipLevel;
    public EquipmentStatus equipStatus;
}

public class EquipmentManager : MonoBehaviour {

    public List<EquipmentItem> axeEquipments = new List<EquipmentItem>();

    public List<EquipmentItem> pickEquipments = new List<EquipmentItem>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool HasRequiredEquipment(EquipmentType type, int level)
    {
        switch(type)
        {
            case EquipmentType.Wood_Cutting:
                if(axeEquipments.Find(x => x.equipStatus == EquipmentStatus.Current).equipLevel >= level)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case EquipmentType.Ore_Mining:
                if (pickEquipments.Find(x => x.equipStatus == EquipmentStatus.Current).equipLevel >= level)
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
}
