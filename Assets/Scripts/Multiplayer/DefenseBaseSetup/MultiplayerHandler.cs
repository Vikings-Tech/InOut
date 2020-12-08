using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerHandler : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Text playerNameText, oppNameText;

    [SerializeField]
    DefenseBaseBoard board;

    [SerializeField]
    DefenseBaseSetup setup;

    private void Start()
    {
        playerNameText.text = PhotonNetwork.NickName;
        playerNameText.color = GetPlayerColor(0);
        if(PhotonNetwork.PlayerListOthers.Length != 0)
        {
            oppNameText.text = PhotonNetwork.PlayerListOthers[0].NickName;
            oppNameText.color = GetPlayerColor(1);
        }
    }

    private Color GetPlayerColor(int i)
    {
        if(i == 0)
            return PhotonNetwork.IsMasterClient ? new Color(1,0,0) : new Color(0,0,1);
        return PhotonNetwork.IsMasterClient ? new Color(0,0,1) : new Color(1,0,0);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        oppNameText.text = newPlayer.NickName;
    }

    public void SendBoardData()
    {
        setup.isReady = true;
        int[] boardLogic = board.GenerateBoardLogic();
        photonView.RPC("RecieveBoardData", RpcTarget.Others, boardLogic);
    }

    [PunRPC]
    void RecieveBoardData(int[] boardData)
    {
        AttackBaseData.attackboardLogic = boardData;
        foreach(int i in AttackBaseData.attackboardLogic)
        {
            Debug.Log(i);
        }
    }
}
