using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class AcceptDenyRequest : MonoBehaviour
{
    public string requestID;
    private DatabaseReference reference;
    private FirebaseAuth user;
    public FriendInfo requestInfo;
    
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseAuth.DefaultInstance;
        
    }
    
    
    private void Accept(FriendInfo friendInfo, Action callback, Action<AggregateException> fallback)
    {
        
            var messageJSON = StringSerializationAPI.Serialize(typeof(FriendInfo), friendInfo);
            var message2JSON = StringSerializationAPI.Serialize(typeof(FriendInfo),
                new FriendInfo(ASRetrieveUserInfo.currentUID, ASRetrieveUserInfo.userName,
                    ASRetrieveUserInfo.currentAvIndex));
            reference.Child("Friends").Child(user.CurrentUser.UserId).Child("Accepted").Push().SetRawJsonValueAsync(messageJSON).ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
                else callback();
            });
            reference.Child("Friends").Child(requestID).Child("Accepted").Push().SetRawJsonValueAsync(message2JSON).ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
                else callback();
            });
            
            reference.Child("Friends").Child(user.CurrentUser.UserId).Child("Requests").Child(requestID).SetValueAsync(null).ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
                else callback();
            });
        
    }

    
    
    private void Reject(Action callback, Action<AggregateException> fallback)
    {
        reference.Child("Friends").Child(user.CurrentUser.UserId).Child("Requests").Child(requestID).SetValueAsync(null).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
        
    public void RequestAccepted()
    {
        Accept(requestInfo,DestroyThis,Debug.Log);
        DestroyThis();
        Debug.Log("accepted");
    }

    public void RequestRejected()
    {
        Reject(DestroyThis,Debug.Log);
        DestroyThis();
        Debug.Log("rejected");
    }
    
}
