using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class ARPlaneSpawner : MonoBehaviour
{
    public GameObject AR_object;
    public Camera AR_Camera;
    public ARRaycastManager raycastManager;
    public List<ARRaycastHit> hits = new List<ARRaycastHit>();
    // Start is called before the first frame update
    public bool tapped  = false;
    public GameObject visual;

    void Start()
    {
        visual.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    raycastManager.Raycast(new Vector2((Screen.width)/2 , (Screen.height)/2), hits);

    if(hits.Count > 0){
        transform.position = hits[0].pose.position;
        transform.rotation = hits[0].pose.rotation;

        if(!visual.activeInHierarchy && !tapped){
            visual.SetActive(true);
        }
    }

        if (Input.GetMouseButtonDown(0) && !tapped)
        {
            Ray ray = AR_Camera.ScreenPointToRay(Input.mousePosition);
            if (raycastManager.Raycast(ray, hits))
            {
                Pose pose = hits[0].pose;
                Instantiate(AR_object, pose.position , pose.rotation);
                tapped = true;
                visual.SetActive(false);
            }
        }

    }
}

