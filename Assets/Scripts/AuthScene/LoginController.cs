using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoginController : MonoBehaviour
{
    public static bool LoggedIn = false;
    public InputField emailInput, passwordInput;
    public GameObject authPanel, registrationPanel,userInfoPanel;
    
    private void Start()
    {
        
        Screen.orientation = ScreenOrientation.Portrait;
    }
    

    
    public void Login()
    {
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(emailInput.text,
            passwordInput.text).ContinueWith((task =>
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

            if (!task.IsCompleted) return;
            LoggedIn = true;
                
                
        }));
        
        Debug.Log("Logged In");
        SceneManager.LoadScene("MainMenuScene");
        
    }
    
    void GetErrorMessage(AuthError errorCode)
    {
        string msg = "";
        msg = errorCode.ToString();
        print(msg);
    }

    
    public void Register()
    {
        authPanel.SetActive(false);
        registrationPanel.SetActive(true);
        userInfoPanel.SetActive(true);
    }
}
