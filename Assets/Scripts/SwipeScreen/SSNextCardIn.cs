using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SSNextCardIn : MonoBehaviour
{
    private bool cardOnTop;

    private SSSwipeAction _swipeAction;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.GetSiblingIndex() == 1)
        {
            cardOnTop = true;
        }
        else
        {
            cardOnTop = false;
        }

        _swipeAction = FindObjectOfType<SSSwipeAction>();
        _swipeAction.cardMoved += nextCardIn;
        if (cardOnTop) return;
        transform.localScale = new Vector3((float)0.8, (float)0.8, (float)0.8);
        transform.GetComponent<Image>().color = new Color(1,1,1,(float) 0.8);
        
        
    }

    private void Update()
    {
        float step = Mathf.SmoothStep((float) 0.8, 1, _swipeAction.distMoved);
        transform.localScale = new Vector3(step,step,step);
        transform.GetComponent<Image>().color = new Color(1,1,1,(float) step);
    }

    public void nextCardIn()
    {
        gameObject.AddComponent<SSSwipeAction>();
        Destroy(this);
        //StartCoroutine(CardUp());

    }

    IEnumerator CardUp()
    {
        var num = 0.8;
        float time = 0;
        while (num < 1)
        {
            time += Time.deltaTime;
            num = Mathf.SmoothStep((float)0.8, 1, time);
            transform.localScale = new Vector3((float)num,(float)num,(float)num);
            transform.GetComponent<Image>().color = new Color(1,1,1,(float) num);
            yield return null;
        }
    }
}
