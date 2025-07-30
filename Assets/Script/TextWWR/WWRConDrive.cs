using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class WWRConDrive : MonoBehaviour
{
    [SerializeField] string testString;
    [SerializeField] TextMeshProUGUI text;


    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest tempWebVar = UnityWebRequest.Get("https://drive.google.com/uc?export=download&id=1hZKhnsoXqga5-W7l7DmJAUMeO4LLmDgG");

        //https://drive.google.com/file/d/1hZKhnsoXqga5-W7l7DmJAUMeO4LLmDgG/view?usp=drive_link
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
