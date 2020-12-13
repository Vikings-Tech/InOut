using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase;
using System;
using UnityEngine.UI;
public class SendFriendRequest : MonoBehaviour
{
    private DatabaseReference reference;
    private FirebaseAuth user;
    public InputField userId;

    private ASRetrieveUserInfo _retrieveUserInfo;
    // Start is called before the first frame update
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        _retrieveUserInfo = FindObjectOfType<ASRetrieveUserInfo>();
        user = FirebaseAuth.DefaultInstance;
    }

    public void SetRequest()
    {
        SendRequest(userId.text,new FriendInfo(user.CurrentUser.UserId,ASRetrieveUserInfo.userName,ASRetrieveUserInfo.currentAvIndex), ()=> Debug.Log("Friend request was sent")
            ,Debug.Log);
    }
    
    public void SendRequest(string receiver,FriendInfo selfInfo, Action callback, Action<AggregateException> fallback)
    {
        var messageJSON = StringSerializationAPI.Serialize(typeof(FriendInfo), selfInfo);
        reference.Child("Friends").Child(receiver).Child("Requests").Child(selfInfo.UserUID).SetRawJsonValueAsync(messageJSON).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }
}
