using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSMovePanels : MonoBehaviour
{
    public GameObject friendRequestPanel;
    // Start is called before the first frame update
    void Start()
    {
        friendRequestPanel.transform.localPosition = new Vector3(-2*Screen.width,0,0);
    }

    public void GetFriendRequestPanel()
    {
        friendRequestPanel.transform.localPosition = Vector3.zero;
    }

    public void GoBack()
    {
        friendRequestPanel.transform.localPosition = new Vector3(-2*Screen.width,0,0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
