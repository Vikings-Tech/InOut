using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase;
using Firebase.Unity.Editor;
using UnityEngine.UI;
public class ASRetrieveUserInfo : MonoBehaviour
{   private DatabaseReference reference;
    private FirebaseAuth user;

    public static string currentUID;
    public static int currentAvIndex;
    public static string userName;

    

    public InputField idTextfield;
    // Start is called before the first frame update
    void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseAuth.DefaultInstance;
        idTextfield.text = user.CurrentUser.UserId;
        RetrieveData(user.CurrentUser.UserId);
    }

    public void RetrieveData(string userUID)
    {
        reference.Child("User_Info").Child(userUID)
            
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    
                    RegistrationInfo userinfo = StringSerializationAPI.Deserialize(typeof(RegistrationInfo), snapshot.GetRawJsonValue()) as RegistrationInfo;
                    userName = userinfo.UserName;
                    currentUID = user.CurrentUser.UserId;
                    currentAvIndex = userinfo.AvatarIndex;
                }
            });
        
    }
    

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
