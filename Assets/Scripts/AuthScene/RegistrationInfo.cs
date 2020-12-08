using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

public class RegistrationInfo
{
    public string UserName;
    public string Name;
    public string Email;
    public int AvatarIndex;
    public string Bio;
    
    public RegistrationInfo()
    {
        UserName = RegistrationHandler.UserName;
        Name = RegistrationHandler.Name;
        Email = RegistrationHandler.EmailID;
        AvatarIndex = ASAvatarSelect.avatarIndex;
        

    }
}
