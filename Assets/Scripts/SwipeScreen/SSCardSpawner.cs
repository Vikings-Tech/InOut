using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSCardSpawner : MonoBehaviour
{
// private SSSwipeAction _SwipeAction;
public GameObject card;
public Transform container;
    // Start is called before the first frame update
    void Start()
    {
        // _SwipeAction = FindObjectOfType<SSSwipeAction>();
        // _SwipeAction.cardMoved += InstantiateNextCard;
    }

    
    public void InstantiateNextCard()
    {
        var newCard = Instantiate(card, new Vector2(0,247), Quaternion.identity);
        newCard.transform.SetParent(container,false);
        newCard.transform.SetAsFirstSibling();
        newCard.AddComponent<SSNextCardIn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Transform>().childCount != 2)
        {
            InstantiateNextCard();
        }
    }
}
