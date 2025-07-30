using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestLoadTextures : MonoBehaviour
{
    [SerializeField] Renderer thisRenderer;

    void Start()
    {
        thisRenderer = GetComponent<Renderer>();
        StartCoroutine(GetTexture());
    }

    IEnumerator GetTexture()
    {
        //UnityWebRequest tempWebVar = UnityWebRequestTexture.GetTexture("https://drive.google.com/uc?export=download&id=1c4fTpKYP40-8XHe5zZa0HySefOjm-WIE");
        UnityWebRequest tempWebVar = UnityWebRequestTexture.GetTexture("https://drive.google.com/uc?export=download&id=1e49LB-RFt0WDlFQb_VHv04Wl1cIYTI6H");
        //https://drive.google.com/file/d/1e49LB-RFt0WDlFQb_VHv04Wl1cIYTI6H/view?usp=drive_link
        yield return tempWebVar.SendWebRequest();

        if (tempWebVar.isNetworkError || tempWebVar.isHttpError)
        {
            Debug.Log(tempWebVar.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)tempWebVar.downloadHandler).texture;
            thisRenderer.material.mainTexture = myTexture;
        }
    }
}
