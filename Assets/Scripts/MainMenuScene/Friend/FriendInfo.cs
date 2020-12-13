using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendInfo 
{
    public string UserUID;
    public string Username;
    public int AvatarIndex;
    public FriendInfo(string UserUID, string Username, int AvatarIndex)
    {
        this.UserUID = UserUID;
        this.Username = Username;
        this.AvatarIndex = AvatarIndex;
    }
}
