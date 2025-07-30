using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TestLoadText : MonoBehaviour
{
    public string testString;
    [SerializeField] Text text;

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest tempWebVar = UnityWebRequest.Get("https://raw.githubusercontent.com/jusete/BubbleQuiz/master/josetextfile.txt");
        
        yield return tempWebVar.SendWebRequest();

        if (tempWebVar.isNetworkError || tempWebVar.isHttpError)
        {
            Debug.Log(tempWebVar.error);
        }
        else
        {
            Debug.Log(tempWebVar.downloadHandler.text);
            testString = tempWebVar.downloadHandler.text;
            text.text = testString;
        }
    }
}
