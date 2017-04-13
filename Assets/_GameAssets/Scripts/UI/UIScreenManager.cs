using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIScreenCanvas
{
    public GameObject uiCanvas;
    public List<UIScreen> allChildScreens = new List<UIScreen>();
}

public class UIScreenManager : MonoBehaviour {

    public List<UIScreenCanvas> allCanvases = new List<UIScreenCanvas>();
    public string currentlyActiveScreenID;

    public string startingUIID;

	public void ActivateUIScreen(string screenID)
    {
        if (currentlyActiveScreenID == screenID)
        {
            Debug.Log("Already active");
        }
        else
        {
            bool uiScreenFound = false;

            for (int i = 0; i < allCanvases.Count; i++)
            {
                bool uiInThisCanvas = false;
                for (int j = 0; j < allCanvases[i].allChildScreens.Count; j++)
                {
                    if (allCanvases[i].allChildScreens[j].thisScreenID == screenID)
                    {
                        uiScreenFound = true;
                        uiInThisCanvas = true;
                        allCanvases[i].allChildScreens[j].ActivateScreen();
                        currentlyActiveScreenID = screenID;
                    }
                    else
                    {
                        allCanvases[i].allChildScreens[j].DeactivateScreen();
                    }
                }

                if (uiInThisCanvas)
                {
                    allCanvases[i].uiCanvas.SetActive(true);
                }
                else
                {
                    allCanvases[i].uiCanvas.SetActive(false);
                }
            }

            if (uiScreenFound == false)
            {
                Debug.Log("Bad ui string " + screenID);
            }
        }
    }
}
