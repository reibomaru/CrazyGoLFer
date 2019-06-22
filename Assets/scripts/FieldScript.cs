using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    public int hallCount;
    public GameObject sthMoving;
    public GameObject sthMoving2;
    public float timer;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        hallCount = BallScript.hallCount;
        ControllField();
    }

    void ControllField()
    {
        switch (hallCount)
        {
            case 3:
                MoveSth(sthMoving);
                break;
            case 9:
                MoveSth(sthMoving2);
                break;
            default:
                break;
        }
    }

    void MoveSth(GameObject sth)
    {
        timer += Time.deltaTime;
        if (timer < 5)
        {
            sth.transform.position += new Vector3(0, 6.0f * Time.deltaTime, 0);
        }
        if (timer > 6)
        {
            sth.transform.position -= new Vector3(0, 6.0f * Time.deltaTime, 0);
        }
        if (timer > 11)
        {
            timer = 0;
        }
    }
}
