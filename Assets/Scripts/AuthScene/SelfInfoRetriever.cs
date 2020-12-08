using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class SelfInfoRetriever : MonoBehaviour
{
    public static string UserName;
    public static string Name;
    public static string Bio;
    public static string Email;
    public static int AvatarIndex;

    private DatabaseReference reference;
    private FirebaseAuth user;
    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseAuth.DefaultInstance;
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
                    UserName = userinfo.UserName;
                    Name = userinfo.Name;
                    Email = userinfo.Email;
                    AvatarIndex = userinfo.AvatarIndex;
                    Bio = userinfo.Bio;

                }
            });
        
    }
}
