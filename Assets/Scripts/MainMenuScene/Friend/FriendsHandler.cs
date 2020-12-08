using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
public class FriendsHandler : MonoBehaviour
{
    
    public ManageFriendRequest _ManageFriendRequest;
    public GameObject FriendPrefab;
    public Transform FriendContainer;
    public Sprite[] avatars;
    
    // Start is called before the first frame update
    void Start()
    {
        
        _ManageFriendRequest.ListenForRequests(InstantiateRequest,Debug.Log);
        
    }

    
    public void InstantiateRequest(FriendInfo friendInfo)
    {
        var newMessage = Instantiate(FriendPrefab, transform.position, Quaternion.identity);
        newMessage.transform.SetParent(FriendContainer,false);
        newMessage.GetComponentInChildren<Image>().sprite = avatars[friendInfo.AvatarIndex];
        newMessage.GetComponentInChildren<Text>().text = friendInfo.Username;
        newMessage.GetComponentInChildren<AcceptDenyRequest>().requestID = friendInfo.UserUID;
        newMessage.GetComponentInChildren<AcceptDenyRequest>().requestInfo = friendInfo;

    }
    
}
