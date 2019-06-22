using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicManager : MonoBehaviour
{
    public AudioSource secondAudioSource;
    public AudioClip NatureLand;
    public AudioClip DotWorld;
    public AudioClip RainyCity;
    public AudioClip WinterTown;
    public static int stage;

    private void Awake()
    {
        secondAudioSource = secondAudioSource.gameObject.GetComponent<AudioSource>();
        stage = TitleSceneManagerScript.stage;
    }

    // Start is called before the first frame update
    void Start()
    {
        SellectMusic();
        secondAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SellectMusic()
    {
        switch (stage)
        {
            case 1:
                secondAudioSource.clip = NatureLand;
                break;
            case 2:
                secondAudioSource.clip = DotWorld;
                break;
            case 3:
                secondAudioSource.clip = RainyCity;
                break;
            case 4:
                secondAudioSource.clip = WinterTown;
                break;
        }
    }
}
