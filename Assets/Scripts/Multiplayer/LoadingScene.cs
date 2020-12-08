using UnityEngine.UI;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    Text LoadingText;

    string loader;

    int dotCount = 3;
    int i = 0;

    float timeBetweenDotUpdates = 1f;
    float currentTime = 0f;

    private void Start()
    {
        loader = "Loading";
        currentTime = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if(Time.timeSinceLevelLoad > currentTime + timeBetweenDotUpdates)
        {
            i++;
            currentTime = Time.timeSinceLevelLoad;
            loader = "Loading";
            if(i > dotCount)
            {
                i = 0;
            }
            for (int t = 0; t < i; t++)
                loader += ".";

            LoadingText.text = loader;
        }
    }
}
