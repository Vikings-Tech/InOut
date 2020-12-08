using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ProgressPanelHandler : MonoBehaviour
{
    Text progressText;

    private void Start()
    {
        progressText = GetComponent<Text>();
        progressText.text = "Connecting...";
        progressText.color = new Color(1,0,0);
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            progressText.text = "Connected";
            progressText.color = new Color(0, 1, 0);
        }
    }
}
