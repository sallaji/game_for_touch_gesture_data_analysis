using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restful : MonoBehaviour
{

    public string WEB_URL = "";


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RestClient.Instance.Get(WEB_URL));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
