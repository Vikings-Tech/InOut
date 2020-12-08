using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefenseBaseTimer : MonoBehaviour
{
    [SerializeField]
    Text Timer;

    [SerializeField]
    MultiplayerHandler handler;

    [SerializeField]
    DefenseBaseSetup setup;

    float defenseTime = 45f;
    float timeLeft = 0f;
    float lastTime = 0f;

    private void Start()
    {
        timeLeft = 45f;
        lastTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if(timeLeft < 0f)
        {
            handler.SendBoardData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if(Time.timeSinceLevelLoad >= lastTime + 1f)
        {
            timeLeft--;
            lastTime = Time.timeSinceLevelLoad;
            Timer.text = timeLeft.ToString();
        }
    }
}
