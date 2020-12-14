using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeRequestInstantiato : MonoBehaviour
{
    public ChallengeHandler _ChallengeHandler;
    public GameObject FriendPrefab;
    public Transform FriendContainer;
    public Sprite[] avatars;
    public MatchCreator creator;
    
    // Start is called before the first frame update
    void Start()
    {
        
        _ChallengeHandler.ListenForRequests(InstantiateRequest,Debug.Log);
        
    }

    
    public void InstantiateRequest(ChallengeInfo friendInfo)
    {
        var newMessage = Instantiate(FriendPrefab, transform.position, Quaternion.identity);
        newMessage.transform.SetParent(FriendContainer,false);
        newMessage.GetComponentInChildren<Image>().sprite = avatars[friendInfo.AvatarIndex];
        Text[] text = newMessage.GetComponentsInChildren<Text>();
        text[0].text = friendInfo.Username;
        text[1].text = friendInfo.GameName;
        newMessage.GetComponentInChildren<AcceptDenyChallenge>().requestID = friendInfo.UserUID;
        newMessage.GetComponentInChildren<AcceptDenyChallenge>().requestInfo = friendInfo;
        newMessage.GetComponentInChildren<AcceptDenyChallenge>().ChallengeCreator = creator;
    }
}
