using System;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class ChallengeHandler : MonoBehaviour
{
    private EventHandler<ChildChangedEventArgs> messageListener;
    private DatabaseReference reference;
    private FirebaseAuth user;

    public GameObject ChallengeRequestsPanel;
    // Start is called before the first frame update
    void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/");
        
        reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Challenges").Child(user.CurrentUser.UserId).Child("Requests");
        user = FirebaseAuth.DefaultInstance;
        ChallengeRequestsPanel.transform.localPosition = new Vector3(2*Screen.width,0,0);
    }

    public void GoToRequestsPanel()
    {
        ChallengeRequestsPanel.transform.localPosition = Vector3.zero;
    }

    public void GoBackRequestsPanel()
    {
        ChallengeRequestsPanel.transform.localPosition = new Vector3(2*Screen.width,0,0);
    }
    public void ListenForRequests(Action<ChallengeInfo> callback,Action<AggregateException> fallback)
    {
        
        void CurrentListener(object o, ChildChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                fallback(new AggregateException(new Exception(args.DatabaseError.Message)));
                Debug.LogError(args.DatabaseError.Message);
            }
            else callback(StringSerializationAPI.Deserialize(typeof(ChallengeInfo),args.Snapshot.GetRawJsonValue()) as ChallengeInfo );

            
        }
        messageListener = CurrentListener;
        
        reference.ChildAdded += messageListener;
    }

    void OnDestroy()
    {
        reference.ChildAdded -= messageListener;
    }
}
