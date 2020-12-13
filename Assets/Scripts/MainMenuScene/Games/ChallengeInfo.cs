using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeInfo 
{
    public string UserUID;
    public string Username;
    public int AvatarIndex;
    public string GameName;
    public string RoomCode;
    public ChallengeInfo(string UserUID, string Username, int AvatarIndex,string gameName,string roomCode)
    {
        this.UserUID = UserUID;
        this.Username = Username;
        this.AvatarIndex = AvatarIndex;
        this.GameName = gameName;
        this.RoomCode = roomCode;

    }
}
