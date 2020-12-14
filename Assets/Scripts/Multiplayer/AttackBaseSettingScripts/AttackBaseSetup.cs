using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class AttackBaseSetup : MonoBehaviourPunCallbacks
{
    const float pausedTimeScale = 0f;

    [SerializeField]
    Text ScoreText;

    int score = 0;

    public bool GameOver = false;
    bool OpponentGameOver = false;
    bool switchScene = false;

    [SerializeField]
    Vector2Int boardSize = new Vector2Int(11, 11);

    [SerializeField]
    AttackBaseBoard board = default;

    [SerializeField]
    GameTileContentFactory tileContentFactory = default;

    static AttackBaseSetup instance;

    public static void EnemyReachedDestination()
    {
        instance.score++;
        instance.ScoreText.text = "Scored: " + instance.score.ToString();
    }

    void Awake()
    {
        instance = this;
        board.Initialize(boardSize, tileContentFactory);
        board.Generate(AttackBaseData.attackboardLogic);
    }

    void OnValidate()
    {
        if (boardSize.x < 2)
        {
            boardSize.x = 2;
        }
        if (boardSize.y < 2)
        {
            boardSize.y = 2;
        }
    }
    private void Update()
    {
        if(GameOver && OpponentGameOver && !switchScene)
        {
            switchScene = true;
            SceneManager.LoadScene("GameOver");
        }
    }

    public void SendScore()
    {
        GameOver = true;
        photonView.RPC("RecieveScoreData", RpcTarget.Others, (object)score);
    }

    [PunRPC]
    void RecieveScoreData(int _score)
    {
        GameOverClass.enemyScore = _score;
        GameOverClass.myScore = score;
        OpponentGameOver = true;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
