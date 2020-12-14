using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchCreator : MonoBehaviourPunCallbacks
{
    public int maxPlayerInRoom = 2;
    public string GameVersion = "1.0";
    string username;
    string roomID;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateRoom(string _roomID, string _username)
    {
        if (string.IsNullOrEmpty(_roomID) || string.IsNullOrEmpty(_username))
        {
            Debug.LogError("One of the Parameters is Null or Empty");
            Debug.LogError("ID: " + _roomID + " Name: " + _username);
            return;
        }
        username = _username;
        roomID = _roomID;
        Connect();
    }

    public void AcceptChallenge(string _roomID, string _username)
    {
        if (string.IsNullOrEmpty(_roomID) || string.IsNullOrEmpty(_username))
        {
            Debug.LogError("One of the Parameters is Null or Empty");
            return;
        }
        username = _username;
        roomID = _roomID;
        Connect();
    }

    public void ChallengeRejected()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Server");
        PhotonNetwork.NickName = username;
        Debug.Log("Creating Room");
        PhotonNetwork.JoinOrCreateRoom(roomID, new RoomOptions
        {
            MaxPlayers = 2
        },
        null,
        null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Player has now joined room: " + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel("WaitingScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
