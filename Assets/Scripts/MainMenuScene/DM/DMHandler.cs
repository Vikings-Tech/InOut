using System;
using UnityEngine;
using Firebase.Database;
using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using UnityEngine.UI;

public class DMHandler : MonoBehaviour
{
    private DatabaseReference reference;
    private FirebaseAuth user;
    private EventHandler<ChildChangedEventArgs> messageListener;
    public event Action backFromDM;
    public static string messageReceiver;
    public Sprite[] avatars;
    public Image avatarImage;
    public Text userName;
    public static int avatarIndex;
    public static string username;
    
    private void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseAuth.DefaultInstance;
        
    }

    public void PostMessage(DMMessage message, Action callback, Action<AggregateException> fallback)
    {
        var messageJSON = StringSerializationAPI.Serialize(typeof(DMMessage), message);
        reference.Child("messages").Child(user.CurrentUser.UserId).Child(messageReceiver).Push().SetRawJsonValueAsync(messageJSON).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
        reference.Child("messages").Child(messageReceiver).Child(user.CurrentUser.UserId).Push().SetRawJsonValueAsync(messageJSON).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted) fallback(task.Exception);
            else callback();
        });
    }

    public void ListenForMessages(Action<DMMessage> callback,Action<AggregateException> fallback)
    {
        
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
                Debug.LogError(args.DatabaseError.Message);
            }
            else callback(StringSerializationAPI.Deserialize(typeof(DMMessage),args.Snapshot.GetRawJsonValue()) as DMMessage );

            
        }
        messageListener = CurrentListener;
        
        reference.Child("messages").Child(user.CurrentUser.UserId).Child(messageReceiver).ChildAdded += messageListener;
    }

    public void RefreshDMPage()
    {
        avatarImage.sprite = avatars[avatarIndex];
        userName.text = username;
    }
    public void StopListeningForMessages()
    {
        backFromDM?.Invoke();
        reference.Child("messages").Child(user.CurrentUser.UserId).Child(messageReceiver).ChildAdded -= messageListener;
    }
    
    
}
