using Firebase;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
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
    
    private DatabaseReference reference;
    private FirebaseAuth user;
    private void Start()
    {
        _authStateHandler = FindObjectOfType<AuthStateHandler>();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseAuth.DefaultInstance;
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


        Invoke("AddSwipePeople",2);
        Invoke("PostToDatabase", 3);
        GoToAvatarSelection();
    }
    
    
    void GetErrorMessage(AuthError errorCode)
    {
        string msg = "";
        msg = errorCode.ToString(); 
        print(msg);
    }

    public void AddSwipePeople()
    {
        reference.Child("User_Info")
            
            .GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.LogError(task.Exception);
                }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    
                    reference.Child("Swipe_List").Child(user.CurrentUser.UserId)
                        .SetRawJsonValueAsync(snapshot.GetRawJsonValue());
                }
            });
        
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
