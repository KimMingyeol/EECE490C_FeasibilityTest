using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FetchImage : MonoBehaviour
{
    public RawImage testImage;
    // public Material testMaterial;
    public GameObject testPlane;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getTexture(testImage));
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator getTexture(RawImage testImage) {
        // string testURL = "http://127.0.0.1:8000/media/test/2022/10/28/profile.PNG";
        string testURL = "https://source.unsplash.com/user/c_v_r/100x100";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(testURL);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            // testImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Material testMaterial = new Material(Shader.Find("Standard"));
            testMaterial.mainTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            testMaterial.SetFloat("_Metallic", 0.0f);
            testMaterial.SetFloat("_Glossiness", 1.0f);
            // testMaterial.SetTexture("_MainTex", ((DownloadHandlerTexture)www.downloadHandler).texture);
            testPlane.GetComponent<MeshRenderer>().material = testMaterial;
        } else {
            Debug.Log("Somthing's gone wrong...");
        }
    }
}
