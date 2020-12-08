using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMMovePanels : MonoBehaviour
{
    public GameObject friendListPanel;
    public GameObject DMPanel;

    private void Start()
    {
        friendListPanel.transform.localPosition = new Vector3(Screen.width,0,0);
        DMPanel.transform.localPosition = new Vector3(2*Screen.width,0,0);
    }

    public void goToFriendList()
    {
        friendListPanel.transform.localPosition = Vector3.zero;
    }
    

    public void backFromFriendList()
    {
        friendListPanel.transform.localPosition = new Vector3(Screen.width,0,0);
    }

    public void goToDMPanel()
    {
        DMPanel.transform.localPosition = Vector3.zero;
    }

    public void backFromDMPanel()
    {
        DMPanel.transform.localPosition = new Vector3(2*Screen.width,0,0);
    }
}
