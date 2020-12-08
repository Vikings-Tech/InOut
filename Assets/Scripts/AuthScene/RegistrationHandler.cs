using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Proyecto26;
public class RegistrationHandler : MonoBehaviour
{
    public GameObject registrationCanvas;
    public GameObject loginCanvas;
    public GameObject avatarSelectionCanvas;
    public GameObject userInfoCanvas;
    
    public InputField RegEmail;
    public InputField password;
    public InputField reEnterPass;
    public InputField userName;
    public InputField nameField;

    public string firebaseAdd;
    public static string UserName;
    public static string EmailID;
    public static string Name;

    private AuthStateHandler _authStateHandler;
    public static bool RegistrationComplete = false;

    private void Start()
    {
        _authStateHandler = FindObjectOfType<AuthStateHandler>();
    }

    public void RegisterUser()
    {
        
        if(RegEmail.text.Equals("") && password.text.Equals(""))
        {
            print("Enter Email/Password");
            return;
        }
        if (password.text != reEnterPass.text)
        {
            
            print("Passwords don't match");
            return;
        }
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(RegEmail.text,
            password.text).ContinueWith((task =>
        {
            if (task.IsCanceled)
            {
                Firebase.FirebaseException e =
                    task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);
                return;
            }
            if (task.IsFaulted)
            {
                Firebase.FirebaseException e =
                    task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
                GetErrorMessage((AuthError)e.ErrorCode);
                return;
            }
            
            if (task.IsCompleted)
            {
                print("Registration COMPLETE");
                RegistrationComplete = true;
                
                
            }
        }));
        
        UserName = userName.text;
        EmailID = RegEmail.text;
        Name = nameField.text;



        Invoke("PostToDatabase", 3);
        GoToAvatarSelection();
    }
    
    
    void GetErrorMessage(AuthError errorCode)
    {
        string msg = "";
        msg = errorCode.ToString(); 
        print(msg);
    }
    
    public void PostToDatabase()
    {
        Debug.Log("yes");
        RegistrationInfo user = new RegistrationInfo();
        RestClient.Put(firebaseAdd+"User_Info/"+_authStateHandler.user.UserId+".json",user);
        
        
    }

    public void GoToAvatarSelection()
    {
        userInfoCanvas.SetActive(false);
        avatarSelectionCanvas.SetActive(true);
    }

    public void GoToLoginCanvas()
    {
        avatarSelectionCanvas.SetActive(false);
        registrationCanvas.SetActive(false);
        loginCanvas.SetActive(true);
    }
    
    
}
