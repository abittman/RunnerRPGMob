using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {

    [Header("References")]
    public ConversationUI convoUI;

    [Header("Conversations")]
    public List<ConversationEvent> allConvoEvents = new List<ConversationEvent>();
    
    public void ActivateEvent(string eventID)
    {
        ConversationEvent eventToSend = allConvoEvents.Find(x => x.conversationUniqueID == eventID);

        if(eventToSend != null)
        {
            convoUI.NewConversationEvent(eventToSend);
        }
    }
}
