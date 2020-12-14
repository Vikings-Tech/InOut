using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Text Player1Name, Player1Score, Player2Name, Player2Score;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Player1Name.text = PhotonNetwork.NickName;
        Player1Name.color = (PhotonNetwork.IsMasterClient) ? new Color(1, 0, 0) : new Color(0,0,1);
        Player1Score.text = GameOverClass.myScore.ToString();
        Player2Name.text = PhotonNetwork.PlayerListOthers[0].NickName;
        Player2Name.color = (PhotonNetwork.IsMasterClient) ? new Color(0, 0, 1) : new Color(1, 0, 0);
        Player2Score.text = GameOverClass.enemyScore.ToString();
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
