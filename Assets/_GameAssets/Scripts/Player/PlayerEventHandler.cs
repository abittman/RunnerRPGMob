using UnityEngine;
using System.Collections.Generic;

public class PlayerEventHandler : MonoBehaviour {

    public List<EnemyBehaviour> listeningEnemies = new List<EnemyBehaviour>();

	public void ChangeLaneEvent(RunningLane rLane)
    {
        Debug.Log("Send out player change lane event");
        for(int i = 0; i < listeningEnemies.Count; i++)
        {
            listeningEnemies[i].PlayerChangesLanes(rLane);
        }
    }
}
