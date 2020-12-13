using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class MMChallengeRequest : MonoBehaviour
{
    public static string gameName;
    public GameObject ChallengeFriendsPanel;
    public Text title;
    
    
    public void SetGameName(string _gameName)
    {
        gameName = _gameName;
        title.text = "Challenge for " + gameName;
        ChallengeFriendsPanel.transform.localPosition = Vector3.zero;
    }

    public void SendChallengeBack()
    {
        ChallengeFriendsPanel.transform.localPosition = new Vector3(2*Screen.width,0,0);
    }
    
    
}
