using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class AcceptDenyChallenge : MonoBehaviour
{
    public string requestID;
    private DatabaseReference reference;
    private FirebaseAuth user;
    public ChallengeInfo requestInfo;
    public MatchCreator ChallengeCreator;
    
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseAuth.DefaultInstance;
    }

    private void Accept(ChallengeInfo friendInfo, Action callback, Action<AggregateException> fallback)
    {
            reference.Child("Challenges").Child(user.CurrentUser.UserId).Child("Requests").Child(requestID).SetValueAsync(null).ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
                else callback();
            });
    }
    
    private void Reject(Action callback, Action<AggregateException> fallback)
    {
        reference.Child("Challenges").Child(user.CurrentUser.UserId).Child("Requests").Child(requestID).SetValueAsync(null).ContinueWith(task =>
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
        ChallengeCreator.AcceptChallenge(requestInfo.RoomCode, ASRetrieveUserInfo.userName);
        DestroyThis();
        Debug.Log("accepted");
    }

    public void RequestRejected()
    {
        Reject(DestroyThis,Debug.Log);
        ChallengeCreator.ChallengeRejected();
        DestroyThis();
        Debug.Log("rejected");
    }
}
