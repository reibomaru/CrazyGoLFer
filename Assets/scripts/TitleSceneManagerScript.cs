using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneManagerScript : MonoBehaviour
{
    public static int hallCount;
    public Image backGround;
    public Image returnButton;
    public Text parRigit;
    public Text bestScoreRigit;
    public Sprite natureLand;
    public Sprite naturelandButton;
    public Sprite dotWorld;
    public Sprite dotWorldButton;
    public Sprite rainyCity;
    public Sprite rainyCityButton;
    public Sprite winterTown;
    public Sprite winterTownButton;
    public AudioClip titleSceneMusicClip;
    public AudioClip buttonSound;
    public AudioClip buttonToNaturelandSound;
    public AudioClip buttonToDotWorldSound;
    public AudioClip buttonToRainyCitySound;
    public AudioClip buttonToStartScreenSound;
    public AudioClip buttonToManualSound;
    public AudioClip returnSound;
    public AudioSource buttonSoundPlayer;
    AudioSource backGroundMusicPlayer;

    float bestScore;
    public static int stage;

    Animator animator;
    public GameObject titleScene;

    public void Awake()
    {
        bestScoreRigit = bestScoreRigit.gameObject.GetComponent<Text>();
        parRigit = parRigit.gameObject.GetComponent<Text>();
        buttonSoundPlayer = buttonSoundPlayer.gameObject.GetComponent<AudioSource>();
        backGroundMusicPlayer = this.gameObject.GetComponent<AudioSource>();
        backGround = backGround.gameObject.GetComponent<Image>();
        returnButton = returnButton.gameObject.GetComponent<Image>();
        animator = titleScene.gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        backGroundMusicPlayer.clip = titleSceneMusicClip;
        backGroundMusicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //以下ボタンの関数
    public void TitleToMenu()
    {
        PlayButtonSound(buttonSound);
        SetScreen(2);
    }

    public void MenuToTitle()
    {
        PlayButtonSound(returnSound);
        SetScreen(1);
    }

    public void ToManualScreen()
    {
        PlayButtonSound(buttonToManualSound);
        SetScreen(4);
    }

    public void ReturnToTitleSceen()
    {
        PlayButtonSound(returnSound);
        SetScreen(1);
    }

    public void MenuToNatureLand()
    {
        PlayButtonSound(buttonToStartScreenSound);
        stage = 1;
        SetInitialValue(1,natureLand,naturelandButton,21);
        SetInitialPositionOfUI(485, 7, 485, -283, 250, -845, 355);
        SetScreen(3);
    }

    public void MenuToDotWorld()
    {
        PlayButtonSound(buttonToStartScreenSound);
        stage = 2;
        SetInitialValue(7, dotWorld, dotWorldButton, 39);
        SetInitialPositionOfUI(485, 52, 485, -210, 240, -845, 355);
        SetScreen(3);
    }

    public void MenuToRainyCity()
    {
        PlayButtonSound(buttonToStartScreenSound);
        stage = 3;
        SetInitialValue(13, rainyCity, rainyCityButton, 40);
        SetInitialPositionOfUI(940, 140, 940, -25, 130, -700, 385);
        SetScreen(3);
    }

    public void MenuToWinterTown()
    {
        PlayButtonSound(buttonToStartScreenSound);
        stage = 4;
        SetInitialValue(19,winterTown,winterTownButton,40);
        SetInitialPositionOfUI(1080, -85, 1080, -430, 150, 750, 385);
        SetScreen(3);
    }

    public void StartToCourceScene()
    {
        StartCoroutine("LoadScene");
    }

    public void StartToMenu()
    {
        SetScreen(2);
        PlayButtonSound(buttonToStartScreenSound);
    }

    //関数

    void SetScreen(int screenNumber)
    {
        animator.SetInteger("screenNumber", screenNumber);
    }

    //ステージごとに最初のhallCount、画面のUIを行う
    public void SetInitialValue(int firstHall, Sprite Source, Sprite buttonSource, int parScore)
    {
        hallCount = firstHall;
        backGround.sprite = Source;
        returnButton.sprite = buttonSource;
        parRigit.text = parScore.ToString();
        if (PlayerPrefs.HasKey("BestScore" + stage))
        {
            bestScore = PlayerPrefs.GetFloat("BestScore" + stage);
            bestScoreRigit.text = bestScore.ToString("f1") + "s";
        }
        else
        {
            bestScoreRigit.text = "NotYet!!";
        }
        //parRigit.transform.position = new Vector3(1150, parRigitPosY, 0);
        //bestScoreRigit.transform.position = new Vector3(1150, bestScorerigitPosY, 0);
    }

    void SetInitialPositionOfUI(int parPosX,int parPosY,int bestsScorePosX,int bestScorePosY,int fontSize, int returnButtonPosX,int returnButtonPosY)
    {
        parRigit.rectTransform.localPosition = new Vector2(parPosX, parPosY);
        bestScoreRigit.rectTransform.localPosition = new Vector2(bestsScorePosX, bestScorePosY);
        parRigit.fontSize = fontSize;
        bestScoreRigit.fontSize = fontSize;
        returnButton.rectTransform.localPosition = new Vector2(returnButtonPosX, returnButtonPosY);
    }

    private IEnumerator LoadScene()
    {
        PlayButtonToCourceSceneSound();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("scene2");
    }

    void PlayButtonToCourceSceneSound()
    {
        switch (stage)
        {
            case 1:
                buttonSoundPlayer.clip = buttonToNaturelandSound;
                break;
            case 2:
                buttonSoundPlayer.clip = buttonToDotWorldSound;
                break;
            case 3:
                buttonSoundPlayer.clip = buttonToRainyCitySound;
                break;
        }
        buttonSoundPlayer.Play();
    }

    void PlayButtonSound(AudioClip clipName)
    {
        buttonSoundPlayer.clip = clipName;
        buttonSoundPlayer.Play();
    }
}
