using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    Vector2 preMousePos;
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            preMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Input.mousePosition;

            Debug.Log("ok");
            Vector2 diff = currentMousePos - preMousePos;
            this.transform.position -= new Vector3(0, diff.y, diff.x) * 1/10;


            preMousePos = currentMousePos;
        }
    }
}
