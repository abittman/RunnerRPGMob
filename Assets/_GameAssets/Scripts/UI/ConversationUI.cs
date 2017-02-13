using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RunnerGameActors
{
    None,
    Player,
    Sprout
}

[System.Serializable]
public class ConversationElement
{
    public RunnerGameActors thisActor;
    public string thisMessage;
}

[System.Serializable]
public class ConversationEvent
{
    public string conversationUniqueID;
    public List<ConversationElement> orderedConversation = new List<ConversationElement>();
}

public class ConversationUI : MonoBehaviour {

    public float nextMessageDelay = 1f;

    public GameObject sproutTopConvoObj;
    public Text sproutTopConvoText;
    public GameObject sproutBotConvoObj;
    public Text sproutBotConvoText;
    public GameObject playerTopConvoObj;
    public Text playerTopConvoText;
    public GameObject playerBotConvoObj;
    public Text playerBotConvoText;

    public ConversationEvent currentConversationEvent;

    float nextMessageTimer = 0f;
    int currentMessageIndex;

    bool messageDisplayed = false;

    // Use this for initialization
    void Start ()
    {
        sproutTopConvoObj.SetActive(false);
        sproutBotConvoObj.SetActive(false);
        playerTopConvoObj.SetActive(false);
        playerBotConvoObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(messageDisplayed)
        {
            nextMessageTimer += Time.deltaTime;
            if(nextMessageTimer >= nextMessageDelay)
            {
                currentMessageIndex++;
                if (currentMessageIndex < currentConversationEvent.orderedConversation.Count)
                {
                    ActivateConversationElement(currentConversationEvent.orderedConversation[currentMessageIndex]);
                    nextMessageTimer = 0f;
                }
                else if(currentMessageIndex == currentConversationEvent.orderedConversation.Count)
                {
                    MoveConversationsAround();
                    nextMessageTimer = 0f;
                }
                else
                {
                    playerTopConvoObj.SetActive(false);
                    sproutTopConvoObj.SetActive(false);
                    messageDisplayed = false;
                    nextMessageTimer = 0f;
                }
            }
        }
	}

    public void NewConversationEvent(ConversationEvent conEvent)
    {
        messageDisplayed = true;
        currentConversationEvent = conEvent;
        currentMessageIndex = 0;
        ActivateConversationElement(conEvent.orderedConversation[0]);
    }

    void ActivateConversationElement(ConversationElement convoElement)
    {
        MoveConversationsAround();

        switch(convoElement.thisActor)
        {
            case RunnerGameActors.Player:
                if (playerTopConvoObj.activeInHierarchy == true
                    || sproutTopConvoObj.activeInHierarchy == true)
                {
                    playerBotConvoObj.SetActive(true);
                    playerBotConvoText.text = convoElement.thisMessage;
                }
                else
                {
                    playerTopConvoObj.SetActive(true);
                    playerTopConvoText.text = convoElement.thisMessage;
                }
                break;
            case RunnerGameActors.Sprout:
                if (playerTopConvoObj.activeInHierarchy == true
                    || sproutTopConvoObj.activeInHierarchy == true)
                {
                    sproutBotConvoObj.SetActive(true);
                    sproutBotConvoText.text = convoElement.thisMessage;
                }
                else
                {
                    sproutTopConvoObj.SetActive(true);
                    sproutTopConvoText.text = convoElement.thisMessage;
                }
                break;
        }
    }

    void MoveConversationsAround()
    {
        //If both top and bot are active, then bot shifts to top
        if (playerBotConvoObj.activeInHierarchy)
        {
            Debug.Log("Move player bot up");
            playerTopConvoText.text = playerBotConvoText.text;
            playerTopConvoObj.SetActive(true);
            playerBotConvoText.text = "";
            playerBotConvoObj.SetActive(false);
            //disable sprout
            sproutTopConvoObj.SetActive(false);
        }
        else if(sproutBotConvoObj.activeInHierarchy)
        {
            Debug.Log("Move sprout bot up");
            sproutTopConvoText.text = sproutBotConvoText.text;
            sproutTopConvoObj.SetActive(true);
            sproutBotConvoText.text = "";
            sproutBotConvoObj.SetActive(false);
            //disable player0
            playerTopConvoObj.SetActive(false);
        }
        else
        {
            //sproutTopConvoObj.SetActive(false);
        }
    }
}
