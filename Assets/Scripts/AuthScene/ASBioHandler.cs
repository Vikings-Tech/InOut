using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using Proyecto26;
public class ASBioHandler : MonoBehaviour
{
    public FirebaseUser user;
    private AuthStateHandler _authStateHandler;
    private RegistrationHandler _registrationHandler;
    public InputField bioField;
    
    void Start()
    {
        _authStateHandler = FindObjectOfType<AuthStateHandler>();
        _registrationHandler = FindObjectOfType<RegistrationHandler>();
        
    }

    public void Submit()
    {
        RegistrationInfo user = new RegistrationInfo();
        user.Bio = bioField.text;
        
        RestClient.Put(_registrationHandler.firebaseAdd+"User_Info/"+_authStateHandler.user.UserId+".json",user);
        gameObject.SetActive(false);
    }
    
    
}
