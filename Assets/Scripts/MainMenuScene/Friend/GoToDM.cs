using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoToDM : MonoBehaviour
{
    private FriendPropertyHolder _friendPropertyHolder;
    private DMHandler _dmHandler;
    private ChatHandler _chatHandler;

    public MMMovePanels _MovePanels;
    // Start is called before the first frame update
    void Start()
    {
        _MovePanels = FindObjectOfType<MMMovePanels>();
        _dmHandler = FindObjectOfType<DMHandler>();
        _chatHandler = FindObjectOfType<ChatHandler>();
        _friendPropertyHolder = GetComponent<FriendPropertyHolder>();
        int avatarIndex = _friendPropertyHolder.AvatarIndex;
        string username = _friendPropertyHolder.Username;
        string userUID = _friendPropertyHolder.UserUID;
        Debug.Log(avatarIndex);
        Debug.Log(username);
        Debug.Log(userUID);
        GetComponent<Button>().onClick.AddListener(()=>{
            goToDM( username,avatarIndex,userUID);
        });
        Debug.Log("Listener Added");
    }

    void goToDM(string username,int avatarIndex,string userUID)
    {
        DMHandler.username = username;
        DMHandler.avatarIndex = avatarIndex;
        DMHandler.messageReceiver = userUID;
        _dmHandler.RefreshDMPage();
        _chatHandler.startListening();
        _MovePanels.goToDMPanel();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
