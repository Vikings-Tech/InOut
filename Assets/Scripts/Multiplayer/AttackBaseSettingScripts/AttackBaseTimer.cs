using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class AttackBaseTimer : MonoBehaviour
{
    [SerializeField]
    Text Timer;

    [SerializeField]
    AttackBaseSetup setup;

    bool dataSent = false;

    float attackTime = 60f;
    float timeLeft = 0f;
    float lastTime = 0f;

    private void Start()
    {
        timeLeft = attackTime;
        lastTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if (timeLeft < 0f && !dataSent)
        {
            dataSent = true;
            setup.SendScore();
        }
        if (Time.timeSinceLevelLoad >= lastTime + 1f && !setup.GameOver)
        {
            timeLeft--;
            lastTime = Time.timeSinceLevelLoad;
            Timer.text = timeLeft.ToString();
        }
    }
}
