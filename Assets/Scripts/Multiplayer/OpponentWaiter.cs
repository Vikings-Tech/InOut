using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

enum BoardGameMode{
    Normal,
    AR
}

public class OpponentWaiter : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Text OpponentReportText;

    [SerializeField]
    string negativeMsg = "Looking For the opponent";

    [SerializeField]
    Color negativeColor = new Color(1f, 235f / 256f, 0f); //FFFEB00

    public float waitingTimeAfterOpponentFound = 10f;
    public float currentTime = 10f;
    public float timeLeft = 10f;

    [SerializeField]
    Color positiveColor = new Color(1f,0f,0f);

    [SerializeField]
    bool isWaitingForOpponent = true, levelLoadCalled = false;

    BoardGameMode mode;

    private void Start()
    {
        mode = BoardGameMode.Normal;
        OpponentReportText.text = negativeMsg;
        OpponentReportText.color = negativeColor;
    }

    private void Update()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1 && isWaitingForOpponent)
        {
            OpponentReportText.text = "Starting: " + timeLeft;
            OpponentReportText.color = positiveColor;
            isWaitingForOpponent = false;
            currentTime = Time.timeSinceLevelLoad;
            //PhotonNetwork.LoadLevel("DefenseBaseSettingScene");
        }
        if (!isWaitingForOpponent && timeLeft > 0f)
        {
            if (currentTime + 1 < Time.timeSinceLevelLoad)
            {
                currentTime = Time.timeSinceLevelLoad;
                timeLeft--;
                OpponentReportText.text = "Starting: " + timeLeft;
            }
        }
        else if (timeLeft < 1f && !levelLoadCalled)
        {
            levelLoadCalled = true;
            PhotonNetwork.LoadLevel((mode == BoardGameMode.Normal)?"DefenseBaseSettingScene":"DefenceBaseAR");
        }
    }

    public void ChangeMode(float value)
    {
        mode = (BoardGameMode)value;
        Debug.Log("Mode Changed to " + mode + " Value" + value);
    }

    public void ChallengeRejected()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
