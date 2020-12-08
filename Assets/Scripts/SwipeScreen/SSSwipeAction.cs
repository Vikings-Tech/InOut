using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SSSwipeAction : MonoBehaviour, IDragHandler,IEndDragHandler, IBeginDragHandler
{
    private Vector2 dragStartLoc;
    private bool swipeLeft;
    public float distMoved;
    public event Action cardMoved;
    

    

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x,transform.localPosition.y);
        
        if (transform.localPosition.x - dragStartLoc.x > 0)
        {
            swipeLeft = false;
            
            transform.localEulerAngles = new Vector3(0,0,Mathf.Lerp(0,-30, (Mathf.Abs(transform.localPosition.x-dragStartLoc.x)/(Screen.width/2))));
        }
        else
        {
            swipeLeft = true;
            
            transform.localEulerAngles = new Vector3(0,0,Mathf.Lerp(0,30, Mathf.Abs(transform.localPosition.x-dragStartLoc.x)/(Screen.width/2)));
        }
        Debug.Log(Mathf.Abs(transform.localPosition.x-dragStartLoc.x)/(Screen.width/2));
        distMoved = Mathf.Abs(transform.localPosition.x-dragStartLoc.x)/(Screen.width/2);
        Debug.Log(distMoved);
    }

    

    public void OnEndDrag(PointerEventData eventData)
    {
        
        if (Mathf.Abs(transform.localPosition.x - dragStartLoc.x) < 0.4 * (Screen.width))
        {
            transform.localPosition = new Vector2(0,transform.localPosition.y);
            Debug.Log(Mathf.Abs(transform.localPosition.x - dragStartLoc.x));
            transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            StartCoroutine(movedImage());
        }

    }

    IEnumerator movedImage()
    {
        float time = 0;
        while (GetComponent<Image>().color != new Color(1,1,1,0))
        {
            time += Time.deltaTime;
            if (swipeLeft)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,transform.localPosition.x-20,4*time),transform.localPosition.y,0);
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,transform.localPosition.x+20,4*time),transform.localPosition.y,0);
            }
            GetComponent<Image>().color = new Color(1,1,1,Mathf.SmoothStep(1,0,4*time));
            yield return null;
        }

        cardMoved?.Invoke();
        Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartLoc = transform.localPosition;
    }
}
