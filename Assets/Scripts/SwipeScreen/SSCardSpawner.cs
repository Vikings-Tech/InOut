using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class SSCardSpawner : MonoBehaviour
{
// private SSSwipeAction _SwipeAction;
public GameObject card;
public Transform container;
private FirebaseAuth _user;
private bool _spawnNext = false;
private readonly string dbURL = "https://gamersgalaxy-4748b-default-rtdb.firebaseio.com/Swipe_List/";
public Sprite[] avatars;
public GameObject firstCard;
public GameObject secondCard;
public GameObject thisIsItText;
private Text[] _cardText;

private bool cardAvailable = false;
    // Start is called before the first frame update
    void Start()
    {
        thisIsItText.SetActive(false);
        firstCard.SetActive(false);
        secondCard.SetActive(false);
        _user = FirebaseAuth.DefaultInstance;
        StartCoroutine(getCardInfo());
    }

    IEnumerator getCardInfo()
    {
        UnityWebRequest Request = UnityWebRequest.Get(dbURL+_user.CurrentUser.UserId+".json");
        
        yield return Request.SendWebRequest();
        if (Request.isNetworkError || Request.isHttpError)
        {
            Debug.LogError(Request.error);
            yield break;
        }
        
        JSONNode Info = JSON.Parse(Request.downloadHandler.text);
        Debug.Log(Info);
        Debug.Log(_user.CurrentUser.UserId);
        if (Info.Count == 0)
        {
            firstCard.SetActive(false);
            secondCard.SetActive(false);
            thisIsItText.SetActive(true);
        }
        else if (Info.Count == 1)
        {
            
            firstCard.SetActive(true);
            _cardText = firstCard.GetComponentsInChildren<Text>();
            _cardText[1].text = Info[0]["UserName"];
            _cardText[2].text = Info[0]["Bio"];
            
            Image[] images = firstCard.GetComponentsInChildren<Image>();
            images[1].sprite = avatars[Info[0]["AvatarIndex"]];
        }
        else if (Info.Count >= 2)
        {
            firstCard.SetActive(true);
            secondCard.SetActive(true);
            cardAvailable = true;
            _cardText = firstCard.GetComponentsInChildren<Text>();
            _cardText[1].text = Info[0]["UserName"];
            _cardText[2].text = Info[0]["Bio"];
            Image[] images = firstCard.GetComponentsInChildren<Image>();
            images[1].sprite = avatars[Info[0]["AvatarIndex"]];
            _cardText = secondCard.GetComponentsInChildren<Text>();
            _cardText[1].text = Info[1]["UserName"];
            _cardText[2].text = Info[1]["Bio"];
            images = secondCard.GetComponentsInChildren<Image>();
            images[1].sprite = avatars[Info[1]["AvatarIndex"]];
            yield return new WaitUntil(()=>_spawnNext);
            for (int i = 2; i < Info.Count; i++) 
            {
                
                var newCard = Instantiate(card, new Vector2(0,247), Quaternion.identity);
                newCard.transform.SetParent(container,false);
                newCard.transform.SetAsFirstSibling();
                newCard.AddComponent<SSNextCardIn>();
                _cardText = newCard.GetComponentsInChildren<Text>();
                _cardText[1].text = Info[i]["UserName"];
                _cardText[2].text = Info[i]["Bio"];
                images = newCard.GetComponentsInChildren<Image>();
                images[1].sprite = avatars[Info[i]["AvatarIndex"]];
                _spawnNext = false;
                yield return new WaitUntil(()=>_spawnNext);
            }

            cardAvailable = false;
            
        }
        yield return null;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Transform>().childCount < 2 && cardAvailable)
        {
            _spawnNext = true;
        }

        if (GetComponent<Transform>().childCount == 0)
        {
            thisIsItText.SetActive(true);
        }
        Debug.Log(GetComponent<Transform>().childCount == 0);
    }
}
