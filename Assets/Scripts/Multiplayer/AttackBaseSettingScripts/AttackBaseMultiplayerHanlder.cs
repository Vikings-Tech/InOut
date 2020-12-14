using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;

public class AttackBaseMultiplayerHanlder : MonoBehaviour
{
    [SerializeField]
    Text playerNameText, oppNameText;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        playerNameText.text = PhotonNetwork.NickName;
        playerNameText.color = GetPlayerColor(0);
        if (PhotonNetwork.PlayerListOthers.Length != 0)
        {
            oppNameText.text = PhotonNetwork.PlayerListOthers[0].NickName;
            oppNameText.color = GetPlayerColor(1);
        }
    }

    private Color GetPlayerColor(int i)
    {
        if (i == 0)
            return PhotonNetwork.IsMasterClient ? new Color(1, 0, 0) : new Color(0, 0, 1);
        return PhotonNetwork.IsMasterClient ? new Color(0, 0, 1) : new Color(1, 0, 0);
    }
}
