using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public string username;
    public List<PostSerializer> posts;
}

public class PostFetcher : MonoBehaviour {
    public Button fetchPostsButton;
    private string hostURL = "http://127.0.0.1:8000";
    private PhotoFetcher photoFetcher;
    private List<string> photoURLs;

    void Start() {
        photoFetcher = transform.GetChild(0).GetComponent<PhotoFetcher>();
        photoURLs = new List<string>();
        fetchPostsButton.onClick.AddListener(initiateFetchPosts);
        // photoURLs.Clear();
    }

    void Update() {

    }

    private void initiateFetchPosts() {
        if (!UserState.logIn) {
            Debug.Log("Fetch Posts: NOT YET!");
            return;
        }

        string api = "/photo_server/fetch/";
        StartCoroutine(fetchPosts(hostURL + api + "?username=" + UserState.username));
    }

    private IEnumerator fetchPosts(string fetchPostsURL) {
        UnityWebRequest fetchRequest = UnityWebRequest.Get(fetchPostsURL);
        fetchRequest.SetRequestHeader("Authorization", "Bearer " + UserState.token);
        Debug.Log("Sending Request to " + fetchPostsURL);
        yield return fetchRequest.SendWebRequest();

        if (fetchRequest.result == UnityWebRequest.Result.Success) {
            FetchPostsSerializer fetchPostsSerializer = JsonConvert.DeserializeObject<FetchPostsSerializer>(fetchRequest.downloadHandler.text);
            Debug.Log("Requester: " + fetchPostsSerializer.username);
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

        fetchRequest.Dispose(); // else, memory leak?
    }
}
