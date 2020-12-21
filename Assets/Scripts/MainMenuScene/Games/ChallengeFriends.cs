using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class ChallengeFriends : MonoBehaviour
{
    private DatabaseReference _reference;
    private FirebaseAuth _user;
    public static string dbURL= "https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/Friends/";
    public GameObject boxPrefab;
    public Transform boxContainer;
    public Sprite[] avatarSprites;
    public MatchCreator ChallengeCreator;
    
    void Start()
    {
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
        _user = FirebaseAuth.DefaultInstance;
        StartCoroutine(GetFriendsList());
    }

    

    IEnumerator GetFriendsList()
    {
        UnityWebRequest Request = UnityWebRequest.Get(dbURL+_user.CurrentUser.UserId+"/Accepted/.json");

        yield return Request.SendWebRequest();
        if (Request.isNetworkError || Request.isHttpError)
        {
            Debug.LogError(Request.error);
            yield break;
        }
        
        JSONNode Info = JSON.Parse(Request.downloadHandler.text);
        Debug.Log(Info);
        for (int i=0;i<Info.Count;i++)
        {    Debug.Log(Info[i]);
            var newBox = Instantiate(boxPrefab, transform.position, Quaternion.identity);
            newBox.transform.SetParent(boxContainer, false);
            Text[] texts = newBox.GetComponentsInChildren<Text>();
            texts[0].text = Info[i]["Username"];    
            
            newBox.GetComponentInChildren<Image>().sprite = avatarSprites[Int16.Parse(Info[i]["AvatarIndex"])];
            FriendPropertyHolder newProp = newBox.GetComponentInParent<FriendPropertyHolder>();
            newProp.Username = Info[i]["Username"];
            newProp.AvatarIndex = Int16.Parse(Info[i]["AvatarIndex"]);
            newProp.UserUID = Info[i]["UserUID"];
            Button sendButton = newBox.GetComponentInChildren<Button>();
            sendButton.onClick.AddListener((() => SetRequest(newProp.UserUID)));


        }
        

        yield return null;
    }

    
    public void SetRequest(string userId)
    {
        
        SendRequest(userId,new ChallengeInfo(_user.CurrentUser.UserId,
                ASRetrieveUserInfo.userName,ASRetrieveUserInfo.currentAvIndex,MMChallengeRequest.gameName,_user.CurrentUser.UserId), 
            ChallengeRequestSentSuccess
            ,Debug.Log);
        Debug.Log("Challenge Sent");

    }

    public void ChallengeRequestSentSuccess()
    {
        ChallengeCreator.CreateRoom( _user.CurrentUser.UserId, ASRetrieveUserInfo.userName);
    }
    
    public void SendRequest(string receiver,ChallengeInfo selfInfo, Action callback, Action<AggregateException> fallback)
    {
        var messageJSON = StringSerializationAPI.Serialize(typeof(ChallengeInfo), selfInfo);
        _reference.Child("Challenges").Child(receiver).Child("Requests").Child(selfInfo.UserUID).SetRawJsonValueAsync(messageJSON).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }
    
    
}
