using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChange : MonoBehaviour
{
    public void GoToScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
