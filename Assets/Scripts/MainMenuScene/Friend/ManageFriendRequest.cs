using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class ManageFriendRequest : MonoBehaviour
{
    private EventHandler<ChildChangedEventArgs> messageListener;
    private DatabaseReference reference;
    private FirebaseAuth user;
    // Start is called before the first frame update
    void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/");
    
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseAuth.DefaultInstance;
    }

    public void ListenForRequests(Action<FriendInfo> callback,Action<AggregateException> fallback)
    {
        
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
                Debug.LogError(args.DatabaseError.Message);
            }
            else callback(StringSerializationAPI.Deserialize(typeof(FriendInfo),args.Snapshot.GetRawJsonValue()) as FriendInfo );

            
        }
        messageListener = CurrentListener;
        
        reference.Child("Friends").Child(user.CurrentUser.UserId).Child("Requests").ChildAdded += messageListener;
    }
    
}
