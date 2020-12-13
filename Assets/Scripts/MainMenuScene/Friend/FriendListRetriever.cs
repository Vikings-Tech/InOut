using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
public class FriendListRetriever : MonoBehaviour
{
    private DatabaseReference _reference;
    private FirebaseAuth _user;
    
    public static string dbURL= "https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/Friends/";
    private string _senderName;
    private string _senderUid;
    private int _senderAvatar;

    public Sprite[] avatarSprites;
    private List<string> Name;
    private List<int> AvatarIndex;
    public GameObject boxPrefab;
    public Transform boxContainer;
    // Start is called before the first frame update
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
            newBox.GetComponentInChildren<Text>().text = Info[i]["Username"];
            newBox.GetComponentInChildren<Image>().sprite = avatarSprites[Int16.Parse(Info[i]["AvatarIndex"])];
            FriendPropertyHolder newProp = newBox.GetComponent<FriendPropertyHolder>();
            newProp.Username = Info[i]["Username"];
            newProp.AvatarIndex = Int16.Parse(Info[i]["AvatarIndex"]);
            newProp.UserUID = Info[i]["UserUID"];
            newBox.AddComponent<GoToDM>(); 
        }

        yield return null;
    }
    
    
}
