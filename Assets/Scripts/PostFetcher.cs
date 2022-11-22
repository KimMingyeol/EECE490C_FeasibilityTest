using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

// Code Style: Redundant comments including the initially generated ones (above Start and Update) removed

public class UserSerializer {
    public string nickname;
}

public class PostSerializer {
    public int id;
    public List<UserSerializer> heart_users;
    public string photo;
    public int captured_year;
    public int captured_month;
    public int captured_day;
    public int captured_hour;
    public int captured_minute;
    public string caption;
}

public class FetchPostsSerializer {
    public string nickname;
    public List<PostSerializer> posts;
}

public class PostFetcher : MonoBehaviour {
    private string hostURL;
    private PhotoFetcher photoFetcher;
    private List<string> photoURLs;

    void Start() {
        hostURL = "http://127.0.0.1:8000";
        photoFetcher = transform.GetChild(0).GetComponent<PhotoFetcher>();
        photoURLs = new List<string>();
        // photoURLs.Clear();
        StartCoroutine(fetchPosts("Nkmg"));
    }

    void Update() {

    }

    private IEnumerator fetchPosts(string nickname) {
        string api = "/photo_server/fetch/";
        string requestURL = hostURL + api + "?nickname=" + nickname;
        UnityWebRequest fetchRequest = UnityWebRequest.Get(requestURL);
        Debug.Log("Sending Request to " + requestURL);
        yield return fetchRequest.SendWebRequest();

        if (fetchRequest.result == UnityWebRequest.Result.Success) {
            FetchPostsSerializer fetchPostsSerializer = JsonConvert.DeserializeObject<FetchPostsSerializer>(fetchRequest.downloadHandler.text);
            Debug.Log("Requester: " + fetchPostsSerializer.nickname);
            foreach (PostSerializer post in fetchPostsSerializer.posts) {
                Debug.Log("Post #" + post.id.ToString());
                foreach (UserSerializer heart_user in post.heart_users) {
                    Debug.Log("- " + heart_user.nickname + " likes this post.");
                }
                Debug.Log("- Photo URL: " + hostURL + post.photo);
                Debug.Log("- Captured Date: " + post.captured_year.ToString() + "/" + post.captured_month.ToString() + "/" + post.captured_day.ToString());
                Debug.Log("- Captured Time: " + post.captured_month.ToString() + ":" + post.captured_minute.ToString());
                Debug.Log("- Caption: " + post.caption);

                photoURLs.Add(hostURL + post.photo);
            }

            photoFetcher.triggerFetch(photoURLs); // Passing photoURLs to the argument: Deep copy? or Shallow copy?
        } else {
            Debug.Log("Error while fetching Posts!");
        }
    }
}
