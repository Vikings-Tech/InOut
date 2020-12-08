using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OpponentWaiter : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Text OpponentReportText;

    [SerializeField]
    string negativeMsg = "Looking For the opponent";

    [SerializeField]
    Color negativeColor = new Color(1f, 235f / 256f, 0f); //FFFEB00

    [SerializeField]
    string positiveMsg = "Opponent Found";

    [SerializeField]
    Color positiveColor = new Color(1f,0f,0f);

    [SerializeField]
    bool isWaitingForOpponent = true;

    private void Start()
    {
        OpponentReportText.text = negativeMsg;
        OpponentReportText.color = negativeColor;
    }

    private void Update()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1 && isWaitingForOpponent)
        {
            OpponentReportText.text = positiveMsg;
            OpponentReportText.color = positiveColor;
            isWaitingForOpponent = false;
            PhotonNetwork.LoadLevel("DefenseBaseSettingScene");
        }
    }
}
