using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestClient : MonoBehaviour
{

    public static RestClient _instance;
    public static RestClient Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<RestClient>();
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = typeof(RestClient).Name;
                    _instance = gameObject.AddComponent<RestClient>();
                    DontDestroyOnLoad(gameObject);
                }
            }
            return _instance;
        }
    }


    public IEnumerator Get(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8
                    .GetString(www.downloadHandler.data);
                    Debug.Log(jsonResult);
                }
            }

        }
    }

    public IEnumerator Post(string url, LogData newLogData)
    {
        var jsonData =
            JsonUtility.ToJson(newLogData);
        using (UnityWebRequest www =
            UnityWebRequest.Post(url, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw
                (System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log(string
                        .Format("new LogData created {0}", newLogData));
                }
            }
        }
    }
}
