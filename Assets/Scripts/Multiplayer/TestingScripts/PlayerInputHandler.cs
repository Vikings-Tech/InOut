using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(InputField))]
public class PlayerInputHandler : MonoBehaviour
{
    const string PlayerNamePrefKey = "PLAYER_NAME";

    private void Start()
    {
        string defaultName = string.Empty;
        InputField inputField = GetComponent<InputField>();
        if(inputField != null)
        {
            if (PlayerPrefs.HasKey(PlayerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("PlayerName is Empty");
        }
        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(PlayerNamePrefKey,value);
    }
}
