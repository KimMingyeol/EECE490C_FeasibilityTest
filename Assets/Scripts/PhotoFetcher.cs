using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// TODO: Photo size adaptation

public class PhotoFetcher : MonoBehaviour {
    private GameObject[] photoPlanes;

    void Start() {
        photoPlanes = GameObject.FindGameObjectsWithTag("Photo");
    }

    void Update() {

    }

    public void triggerFetch(List<string> photoURLs) {
        int idx = 0;
        foreach (string photoURL in photoURLs) {
            if (idx >= photoPlanes.Length)
                break;
            StartCoroutine(fetchAndDisplayPhotos(photoURL, photoPlanes[idx]));
            idx++;
        }
    }

    private IEnumerator fetchAndDisplayPhotos(string photoURL, GameObject photoPlane) {
        UnityWebRequest fetchRequest = UnityWebRequestTexture.GetTexture(photoURL);
        yield return fetchRequest.SendWebRequest();

        if (fetchRequest.result == UnityWebRequest.Result.Success) {
            Material planeMaterial = new Material(Shader.Find("Standard"));
            planeMaterial.mainTexture = ((DownloadHandlerTexture)fetchRequest.downloadHandler).texture;
            planeMaterial.SetFloat("_Metallic", 0.0f);
            planeMaterial.SetFloat("_Glossiness", 1.0f);
            photoPlane.GetComponent<MeshRenderer>().material = planeMaterial;
        } else {
            Debug.Log("Error while fetching Photos!");
        }

    }
}
