using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Proyecto26;
using Firebase.Auth;
using Models;
using UnityEditor;

public class SSFetchFromDB : MonoBehaviour
{
    FirebaseAuth user;
    private RegistrationHandler _registrationHandler;

    private string firebaseAdd = "https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/";

    private void LogMessage(string title, string message) {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog (title, message, "Ok");
#else
		Debug.Log(message);
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        
        user = FirebaseAuth.DefaultInstance;
        
        // RestClient.Get<RegistrationInfo>(firebaseAdd + "User_Info.json").Then(res =>
        // {
        //     Debug.Log(res.UserName);
        //
        // }).Progress();
        Debug.Log("Done");
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
