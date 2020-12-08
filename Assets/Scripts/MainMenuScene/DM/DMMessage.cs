using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMMessage
{
    
    public string senderName;
    public string userUID;
    public string message;
    public int senderAvatar;
    public DMMessage(string senderName, string message, string userUID,int senderAvatar)
    {
        this.senderName = senderName;
        this.message = message;
        this.senderAvatar = senderAvatar;
        this.userUID = userUID;

    }
}
