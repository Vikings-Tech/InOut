using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;


public class ASAvatarSelect : MonoBehaviour
{
    public Sprite[] avatars;
    public Image avatarHolder;
    
    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject bioPanel;
    
    public static int avatarIndex;

    private AuthStateHandler _authStateHandler;
    private RegistrationHandler _registrationHandler;
    // Start is called before the first frame update
    void Start()
    {
        _authStateHandler = FindObjectOfType<AuthStateHandler>();
        _registrationHandler = FindObjectOfType<RegistrationHandler>();
        avatarIndex = 0;
        avatarHolder.sprite = avatars[avatarIndex];
        leftButton.GetComponent<Button>().interactable = false;
    }

    public void right()
    {
        avatarIndex += 1;
        avatarHolder.sprite = avatars[avatarIndex];
        if (avatarIndex == 1)
        {
            leftButton.GetComponent<Button>().interactable = true;
        }

        if (avatarIndex == avatars.Length - 1)
        {
            rightButton.GetComponent<Button>().interactable = false;
        }
    }

    public void left()
    {
        avatarIndex -= 1;
        avatarHolder.sprite = avatars[avatarIndex];
        if (avatarIndex == avatars.Length-2)
        {
            rightButton.GetComponent<Button>().interactable = true;
        }

        if (avatarIndex == 0)
        {
            leftButton.GetComponent<Button>().interactable = false;
        }
    }

    public void Select()
    {
        RegistrationInfo user = new RegistrationInfo();
        
        RestClient.Put(_registrationHandler.firebaseAdd+"User_Info/"+_authStateHandler.user.UserId+".json",user);
        
        bioPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    
    
}
