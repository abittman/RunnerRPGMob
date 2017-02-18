using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftedItem
{
    public RunnerResource craftedItem;

    public List<RunnerResource> resourcesRequiredToCraft;

    public bool oneCraftOnly;
    public bool craftedStatus;

    //Whether or not crafting the item unlocks it (adds to all resources)
    //  or whether it just adds it to the bag (potentially temporary)
    public bool safeItem = true;
}
