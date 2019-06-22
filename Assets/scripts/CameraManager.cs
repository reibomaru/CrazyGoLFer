using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraManager : MonoBehaviour
{
    public GameObject ball;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = ball.transform.position + new Vector3(80, 10, 30);
    }
}
