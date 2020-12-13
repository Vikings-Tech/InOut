using System.Collections;
using System.Collections.Generic;
using Firebase;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;
using Proyecto26;
using SimpleJSON;
using UnityEngine.Networking;

public class ASBioHandler : MonoBehaviour
{
    private DatabaseReference reference;
    public FirebaseUser user;
    private AuthStateHandler _authStateHandler;
    private RegistrationHandler _registrationHandler;
    public InputField bioField;
    private string dbURL = "https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/User_Info";
    
    void Start()
    {   FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
        _authStateHandler = FindObjectOfType<AuthStateHandler>();
        _registrationHandler = FindObjectOfType<RegistrationHandler>();
        
    }

    public void Submit()
    {
        RegistrationInfo user = new RegistrationInfo();
        user.Bio = bioField.text;
        user.userUID = _authStateHandler.user.UserId;
        RestClient.Put(_registrationHandler.firebaseAdd+"User_Info/"+_authStateHandler.user.UserId+".json",user);
        StartCoroutine(AddToSwipeList());
        

        
    }

    IEnumerator AddToSwipeList()
    {
        UnityWebRequest Request = UnityWebRequest.Get(dbURL+".json");

        yield return Request.SendWebRequest();
        if (Request.isNetworkError || Request.isHttpError)
        {
            Debug.LogError(Request.error);
            yield break;
        }
        
        JSONNode Info = JSON.Parse(Request.downloadHandler.text);
        RegistrationInfo user = new RegistrationInfo();
        user.Bio = bioField.text;
        user.userUID = _authStateHandler.user.UserId;
        for (int i = 0; i < Info.Count; i++)
        {
            Debug.Log(Info[i]["userUID"]);
            if (Info[i]["userUID"] != _authStateHandler.user.UserId && Info[i]["userUID"] != "")
            {    Debug.Log("In");
                var messageJSON = StringSerializationAPI.Serialize(typeof(RegistrationInfo), user);
                reference.Child("Swipe_List").Child(Info[i]["userUID"]).Push().SetRawJsonValueAsync(messageJSON);
            }
        }
        
        gameObject.SetActive(false);
        _registrationHandler.GoToLoginCanvas();
        yield return null;
    }
    
    
}
