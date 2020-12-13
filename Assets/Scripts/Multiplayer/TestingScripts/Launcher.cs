using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    [SerializeField]
    byte maxPlayersInRoom = 2;

    [SerializeField]
    private GameObject UsernameInputPanel;
    [SerializeField]
    private GameObject ProgressPanel;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        UsernameInputPanel.SetActive(true);
        ProgressPanel.SetActive(false);
    }

    private string GenerateRoomName()
    {
        string roomName = PhotonNetwork.NickName;
        return roomName;
    }

    public void Connect()
    {
        ProgressPanel.SetActive(true);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.GameVersion = gameVersion;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ProgressPanel.SetActive(false);
        Debug.LogError("Disconnected from the server");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Joining a Random Room was failed\n Creating a new Room and Joining");
        PhotonNetwork.CreateRoom(GenerateRoomName(), new RoomOptions{
            MaxPlayers = maxPlayersInRoom
        });
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("WaitingScene");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room : " + PhotonNetwork.CurrentRoom.Name);
        LoadArena();
    }
}
