using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class ChatHandler : MonoBehaviour
{
    public DMHandler database;
    private string senderName;
    private string senderUID;
    
    public InputField message;
    private int senderAvatar;

    public GameObject messagePrefab;
    public Transform messagesContainer;
    private void Start()
    {
        
        senderUID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        
    }

    public void startListening()
    {
        database.ListenForMessages(InstantiateMessage,Debug.Log);
    }
    public void SendMessage()
    {
        database.PostMessage(new DMMessage(senderName,message.text,senderUID,senderAvatar),()=> Debug.Log("Message was sent")
        ,Debug.Log);
        
    }

    private void InstantiateMessage(DMMessage message)
    {
        var newMessage = Instantiate(messagePrefab, transform.position, Quaternion.identity);
        newMessage.transform.SetParent(messagesContainer,false);
        newMessage.GetComponent<Text>().text = $"{message.message}";
        Debug.Log(message.senderName);
        Debug.Log(senderUID);
        if (message.userUID == senderUID)
        {
            Debug.Log("Yes");
            newMessage.GetComponent<Text>().alignment = TextAnchor.MiddleRight;
        }
    }
}
