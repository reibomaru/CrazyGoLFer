using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NCMB;

public class BallScript : MonoBehaviour
{
    public static int stage;
    public static int hallCount;
    int count;
    int shotStep;
    int strokesCount;
    float k;
    float powerTimer;
    float power;
    float bestScore;
    float scoreTime;
    float lastScore;
    float MaxPower;
    float swingPower;
    public Camera mainCamera;
    public Camera freeCamera;
    public Image powerMeter;
    public Image powerMeterBack;
    public Image right;
    public Image left;
    public Image pauseButton;
    public Image tapToNext;
    public Image backGroundImage;
    public Image freeCameraBackGroundImage;
    public Image waterHazrd;
    public GameObject arrow;
    public GameObject gizmo;
    public GameObject ball;
    public GameObject player;
    public GameObject playerImage;
    public GameObject NatureLand;
    public GameObject DotWorld;
    public GameObject RainyCity;
    public GameObject WinterTown;
    public GameObject RainEffect;
    public Canvas playerUI;
    public Canvas pauseUI;
    public Canvas canvasWorld;
    public Sprite natureLandBackGround;
    public Sprite dotWorldBackGround;
    public Sprite rainyCityBackGround;
    public Sprite winterTownBackGround;
    public Text storokesCountText;
    public Button PlayAgainButton;
    public Button ToMenuButton;
    public Button returnButton;
    public Button freeCameraButton;
    public Text lastScoreRigit;
    public Text bestScoreRigit;
    public Text strokesRigit;
    public Text scoreTimeText;
    bool isTouchingGround;
    bool isTouchingGreen;
    bool CheckResult = false;
    bool isTime = true; //タイマーを逆戻りするために使う
    bool isRightKey = false;
    bool isLeftKey = false;
    bool timerSwicher;
    Rigidbody rb;
    Transform tf;
    Animator animator;
    Animator UIAnimator;
    AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioClip shotSound;
    public AudioClip putterSound;
    public AudioClip boundSoundFairWay;
    public AudioClip boundSoundBunker;
    public AudioClip boundSoundWater;
    public AudioClip obSound;
    public AudioClip clearSound;



    private void Awake()
    {
        rb = ball.GetComponent<Rigidbody>();
        tf = ball.GetComponent<Transform>();
        stage = TitleSceneManagerScript.stage;
        hallCount = TitleSceneManagerScript.hallCount;
        backGroundImage = backGroundImage.GetComponent<Image>();
        freeCameraBackGroundImage = freeCameraBackGroundImage.GetComponent<Image>();
        audioSource = ball.gameObject.GetComponent<AudioSource>();
        audioSource2 = audioSource2.gameObject.GetComponent<AudioSource>();
        animator = playerImage.gameObject.GetComponent<Animator>();
        UIAnimator = playerUI.gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxPower = 120;
        count = 0;
        arrow.gameObject.SetActive(true);
        PutPlayerOnStartPosition();
        timerSwicher = true;
        SetBackGround();
    }

    void Update()
    {
        switch (count)
        {
            case 0://ショットの強さと方向を決定する（カウント０）
                DeterminePowerAndDirestionOfShot();
                break;
            case 1://決定したパワーと方向を元にショットを打つ（カウント１）
                StartCoroutine("Shot");
               break;
            case 2:
                StopBall();
                break;
            case 3://グリーン上でアプローチをする方向と強さを指定
                DeterminePowerAndDirectionOfPutter();
                break;
            case 4://決定したパワーと方向をもとにパターを打つ
                StartCoroutine("Putter");
                break;
        }

        //ゴールまでにかかった時間を計る
        Timer();

        //UIの表示
        storokesCountText.text = strokesCount.ToString();
        scoreTimeText.text = scoreTime.ToString("f1");

        //GameResultUIの表示
        if (CheckResult)
        {
            Debug.Log(bestScore);
            Debug.Log(scoreTime);
            DetermineScene();
            DetermineBestScore();
            SaveDatas();
            ShowScores();
            LoadNextScene();
        }

        //ボールが外に落ちた時処理
        if (ball.transform.position.y < -40)
        {
            ReturnFromOb();
        }
    }




    void Timer()
    {
        if (timerSwicher)
        {
            scoreTime += Time.deltaTime;
        }
    }


    void DeterminePowerAndDirestionOfShot()
    {
        animator.SetBool("ShotBallRight", false);
        animator.SetBool("ShotBallLeft", false);
        animator.SetBool("PutterBallRight", false);
        animator.SetBool("PutterBallLeft", false);
        arrow.gameObject.SetActive(true);
        playerImage.SetActive(true);
        player.gameObject.transform.position = ball.gameObject.transform.position + new Vector3(-0.5f, 0, 0); //playerの位置をボールのところまで持っていく
        gizmo.gameObject.transform.position = ball.gameObject.transform.position; //照準の矢印をボールのところまで持っていく
        DeterminePower(MaxPower, 1);
        DetermineDirection();
    }

    void DeterminePowerAndDirectionOfPutter()
    {
        animator.SetBool("ShotBallRight", false);
        animator.SetBool("ShotBallLeft", false);
        animator.SetBool("PutterBallRight", false);
        animator.SetBool("PutterBallLeft", false);
        arrow.gameObject.SetActive(true);
        player.gameObject.transform.position = ball.gameObject.transform.position + new Vector3(-0.5f, 0, 0); //playerの位置をボールのところまで持っていく
        gizmo.gameObject.transform.position = ball.gameObject.transform.position; //照準の矢印をボールのところまで持っていく
        DeterminePower(MaxPower * 0.6f, 4);//MaxPowerあとで変えないといけない。
        DeterminePutterDirection();
    }

    void StopBall()
    {
        if (rb.IsSleeping()) //条件判定よく考える
        {
            if (isTouchingGround)
            {
                rb.transform.rotation = Quaternion.Euler(45, 0, 0);
                count = 0;
            }
            //グリーン上でボールが停止してプレイヤーがパターを打てるようにする
            if (isTouchingGreen)
            {
                rb.transform.rotation = Quaternion.Euler(90, 0, 0);
                count = 3;
            }
        }
    }

    private IEnumerator Shot()
    {
        if (ball.transform.rotation.x > 0)
        {
            animator.SetBool("ShotBallRight", true);
            Debug.Log("shotRight");
        }
        else
        {
            animator.SetBool("ShotBallLeft", true);
            Debug.Log("shotLeft");
        }

        yield return new WaitForSeconds(0.2f);

        rb.angularDrag = 1.2f;
        rb.drag = 0.3f;
        arrow.SetActive(false);
        rb.AddForce(ball.transform.up * swingPower, ForceMode.Impulse);
        swingPower = 0;
        //strokesCount++;
        SellectClipAndPlayAudiSource2(shotSound);
        Debug.Log("shot");
        count = 2;
    }

    private IEnumerator Putter()
    {
        if (ball.transform.rotation.x > 0)
        {
            animator.SetBool("PutterBallRight", true);
            Debug.Log("putterRight");
        }
        else
        {
            animator.SetBool("PutterBallLeft", true);
            Debug.Log("putterLeft");
        }

        yield return new WaitForSeconds(0.1f);

        rb.angularDrag = 1.2f;
        rb.drag = 0.3f;
        arrow.SetActive(false);
        rb.AddForce(ball.transform.up * swingPower, ForceMode.Impulse);
        swingPower = 0;
        //strokesCount++;
        SellectClipAndPlayAudiSource2(putterSound);
        count = 2;
    }

    //ショットの強さを決める関数
    void DeterminePower(float maxPower, int nextAction)
    {
        switch (shotStep)
        {
            case 1:
                if (isTime == true)
                {
                    powerTimer += Time.deltaTime;
                    if (powerTimer > 2.0)
                    {
                        isTime = false;
                    }
                }
                if (isTime == false)
                {
                    powerTimer -= Time.deltaTime;
                    if (powerTimer < 0f)
                    {
                        isTime = true;
                    }
                }
                k = powerTimer * 45 - 90;
                power = Mathf.Sin(k * Mathf.Deg2Rad) + 1;
                //メータ表示
                powerMeter.fillAmount = power / 1;
                //スウィングパワーを設定
                swingPower = power * maxPower;
                break;
            case 2:
                powerTimer = 0;
                power = 0;
                shotStep = 0;
                strokesCount++;
                count = nextAction;
                break;
            default:
                break;
        }
    }


    //ショットの向きを決める関数
    void DetermineDirection()
    {
        if (Input.GetKey("right") || isRightKey == true)
        {
            gizmo.transform.RotateAround(gizmo.transform.position, new Vector3(1, 0, 0), Time.deltaTime * 180);
        }
        if (Input.GetKey("left") || isLeftKey == true)
        {
            gizmo.transform.RotateAround(gizmo.transform.position, new Vector3(-1, 0, 0), Time.deltaTime * 180);
        }
        ball.transform.rotation = gizmo.transform.rotation;
    }

    //パターの方向を決める関数
    void DeterminePutterDirection()
    {
        if (Input.GetKeyDown("right") || isRightKey == true)
        {
            gizmo.transform.rotation = Quaternion.Euler(90, 0, 0);
            ball.transform.rotation = gizmo.transform.rotation;
        }
        if (Input.GetKeyDown("left") || isLeftKey == true)
        {
            gizmo.transform.rotation = Quaternion.Euler(-90, 0, 0);
            ball.transform.rotation = gizmo.transform.rotation;
        }
    }

    //初期値に戻すための関数
    private void Reset(int x)
    {
        hallCount = x * 6 - 5;
        arrow.gameObject.SetActive(true);
        scoreTime = 0;
        timerSwicher = true;
        strokesCount = 0;
        rb.velocity = Vector3.zero;
        rb.angularDrag = 1000;
        rb.drag = 1000;
        PutPlayerOnStartPosition();
    }

    //ボールを初期位置に持っていくための関数
    public void PutPlayerOnStartPosition()
    {
        switch (hallCount)
        {
            case 1:
                tf.position = new Vector3(0, -0.1f, -9.4f);
                break;
            case 2:
                tf.position = new Vector3(0, -0.1f, 282f);
                break;
            case 3:
                tf.position = new Vector3(0, -0.1f, 610f);
                break;
            case 4:
                tf.position = new Vector3(0, 121f, 916f);
                break;
            case 5:
                tf.position = new Vector3(0, -1.1f, 1141f);
                break;
            case 6:
                tf.position = new Vector3(0, 21f, 1337f);
                break;
            case 7:
                tf.position = new Vector3(0, 21f, 1755f);
                break;
            case 8:
                tf.position = new Vector3(0, 147f, 2054f);
                break;
            case 9:
                tf.position = new Vector3(0, 72f, 1887f);
                break;
            case 10:
                tf.position = new Vector3(0, 108f, 2200f);
                break;
            case 11:
                tf.position = new Vector3(0, 117f, 2471f);
                break;
            case 12:
                tf.position = new Vector3(0, 233f, 2186f);
                break;
            case 13:
                tf.position = new Vector3(0, 28f, 3052f);
                break;
            case 14:
                tf.position = new Vector3(0, 128f, 3356f);
                break;
            case 15:
                tf.position = new Vector3(0, 40f, 3771f);
                break;
            case 16:
                tf.position = new Vector3(-24.3f, 123f, 3082f);
                break;
            case 17:
                tf.position = new Vector3(-24.3f, 163.8f, 3380f);
                break;
            case 18:
                tf.position = new Vector3(-24.3f, 128f, 3950f);
                break;
            default:
                Debug.Log("ホールが読み込めていない");
                hallCount = 1;
                break;
        }
        count = 0;
        rb.angularDrag = 1.2f;
        rb.drag = 0.3f;
        gizmo.transform.rotation = Quaternion.Euler(45, 0, 0);
        ball.transform.rotation = gizmo.transform.rotation;
    }


    void SellectClipAndPlayAudiSource1(AudioClip playingSound)
    {
        audioSource.clip = playingSound;
        audioSource.Play();
    }

    void SellectClipAndPlayAudiSource2(AudioClip playingSound)
    {
        audioSource2.clip = playingSound;
        audioSource2.Play();
    }

    void ReturnFromOb()
    {
        SellectClipAndPlayAudiSource2(obSound);
        StartCoroutine("OutOfBounds");
    }

    //ボールがどの地面に触れているのかを判定するための関数

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "normal")
        {
            isTouchingGround = true;
            SellectClipAndPlayAudiSource1(boundSoundFairWay);
        }
        if(collision.gameObject.tag == "green")
        {
            isTouchingGreen = true;
            SellectClipAndPlayAudiSource1(boundSoundFairWay);
        }
        if (collision.gameObject.tag == "bunker")
        {
            isTouchingGround = true;
            SellectClipAndPlayAudiSource1(boundSoundBunker);
            //バンカーの処理
            rb.angularDrag = 1000;
            rb.drag = 1000;
            rb.transform.rotation = Quaternion.Euler(45, 0, 0);
            count = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "normal" || collision.gameObject.tag == "bunker" || collision.gameObject.tag == "green")
        {
            isTouchingGround = false;
        }
    }

    //ホールアウトと池ぽちゃの処理

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "goal")
        {
            rb.angularDrag = 1000;
            rb.drag = 1000;
            SellectClipAndPlayAudiSource2(clearSound);
            PlayerPrefs.SetInt("playerScore", strokesCount);
            timerSwicher = false;
            UIAnimator.SetBool("CheckResult", true);
            CheckResult = true;
            canvasWorld.gameObject.SetActive(false);
            Debug.Log("GameFinish!!");
        }
        if(other.gameObject.tag == "water")
        {
            SellectClipAndPlayAudiSource2(boundSoundWater);
            waterHazrd.gameObject.SetActive(true);
            rb.angularDrag = 100;
            rb.drag = 15f;
            //池ぽちゃの処理(0.5秒後)
            StartCoroutine("WaterHazard");
        }
    }

    private IEnumerator WaterHazard()
    {
        yield return new WaitForSeconds(0.5f);
        waterHazrd.gameObject.SetActive(false);
        strokesCount += 2;
        rb.velocity = Vector3.zero;
        rb.angularDrag = 1000;
        ball.gameObject.transform.position = gizmo.gameObject.transform.position;
        rb.transform.rotation = Quaternion.Euler(45, 0, 0);
        count --;
    }

    private IEnumerator OutOfBounds()
    {
        yield return new WaitForSeconds(0.5f);
        strokesCount += 2;
        rb.velocity = Vector3.zero;
        rb.angularDrag = 1000;
        ball.gameObject.transform.position = gizmo.gameObject.transform.position;
        rb.transform.rotation = Quaternion.Euler(45, 0, 0);
        count = 0;
    }

    //以下GameResultのUI

    void DetermineBestScore()
    {
        //ベストスコアの読み込み
        if (PlayerPrefs.HasKey("BestScore" + stage))
        {
            bestScore = PlayerPrefs.GetFloat("BestScore" + stage);
        }
        else
        {
            bestScore = lastScore;
        }
        //ベストスコアとラストスコアの比較
        if (bestScore >= lastScore)
        {
            bestScore = lastScore;
        }
    }

    void ShowScores()
    {
        lastScoreRigit.text = scoreTime.ToString("f1");
        bestScoreRigit.text = bestScore.ToString("f1");
        strokesRigit.text = strokesCount.ToString();
    }

    //ベストスコアを保存し、currentScoreをリセットする
    void SaveDatas()
    {
        switch (hallCount)
        {
            case 6:
            case 12:
            case 18:
                PlayerPrefs.SetFloat("BestScore" + stage, bestScore);
                break;
            default:
                break;
        }
    }

    //UIの配置を決めるついでに、最終ホールの時はcurrentScoreを決定する
    void DetermineScene()
    {
        switch (hallCount)
        {
            case 6:
            case 12:
            case 18:
                lastScore = scoreTime;
                SetScreen(true, true, false);
                break;
            default:
                SetScreen(false, false, true);
                break;
        }
    }

    void LoadNextScene()
    {
        switch (hallCount)
        {
            case 6:
            case 12:
            case 18:
                break;
            default:
                SetScreen(false, false, true);
                if (Input.GetKey("space") || Input.touchCount > 0)
                {
                    UIAnimator.SetBool("CheckResult", false);
                    isTouchingGreen = false;
                    hallCount++;
                    PutPlayerOnStartPosition();
                    CheckResult = false;
                    timerSwicher = true;
                    canvasWorld.gameObject.SetActive(true);
                }
                break;
        }
    }

    void SetScreen(bool againButton, bool menuButton, bool playNextButton)
    {
        PlayAgainButton.gameObject.SetActive(againButton);
        ToMenuButton.gameObject.SetActive(menuButton);
        tapToNext.gameObject.SetActive(playNextButton);
    }

    //背景画像を変えるための関数
    void SetBackGround()
    {
        switch (stage)
        {
            case 1:
                ChangeBackGroundImage(natureLandBackGround);
                NatureLand.SetActive(true);
                break;
            case 2:
                ChangeBackGroundImage(dotWorldBackGround);
                DotWorld.SetActive(true);
                break;
            case 3:
                ChangeBackGroundImage(rainyCityBackGround);
                RainEffect.SetActive(true);
                RainyCity.SetActive(true);
                break;
            case 4:
                ChangeBackGroundImage(winterTownBackGround);
                WinterTown.SetActive(true);
                break;
        }
    }

    void ChangeBackGroundImage(Sprite source)
    {
        backGroundImage.sprite = source;
        freeCameraBackGroundImage.sprite = source;
    }

    //フリーカメラの時の画面を変更する関数
    void SetFreeCameraUI(bool set)
    {
        mainCamera.gameObject.SetActive(set);
        powerMeterBack.gameObject.SetActive(set);
        right.gameObject.SetActive(set);
        left.gameObject.SetActive(set);
        pauseButton.gameObject.SetActive(set);
    }

    //ボタンの実装(プレイ中のUI)
    public void GetKeyUpRight()
    {
        isRightKey = false;
    }
    public void GetKeyDownRight()
    {
        isRightKey = true;
    }
    public void GetKeyUpLeft()
    {
        isLeftKey = false;
    }
    public void GetKeyDownLeft()
    {
        isLeftKey = true;
    }
    public void GetKeyUpSpace()
    {
        if (count == 0 || count == 3)
        {
            shotStep = 2;
        }
    }
    public void GetKeyDownspace()
    {
        if(count == 0 || count == 3)
        {
            shotStep = 1;
        }
    }
    public void SetCameraMode()
    {
        freeCamera.gameObject.transform.position = mainCamera.gameObject.transform.position;
        returnButton.gameObject.SetActive(true);
        SetFreeCameraUI(false);
        Time.timeScale = 0f;
    }

    public void SetMainCamera()
    {
        SetFreeCameraUI(true);
        Time.timeScale = 1f;
        returnButton.gameObject.SetActive(false);
    }

    public void Pause()
    {
        pauseUI.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        freeCameraButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    //ボタンの実装(GameResultでのUI)
    public void PlayAgain()
    {
        UIAnimator.SetBool("CheckResult", false);
        Reset(stage);
        PutPlayerOnStartPosition();
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("scene1");
    }

    //ボタンの実装(PauseでのUI)
    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("scene1");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        pauseButton.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        freeCameraButton.gameObject.SetActive(true);
        Reset(stage);
        PutPlayerOnStartPosition();
    }

    public void Resume()
    {
        pauseButton.gameObject.SetActive(true);
        pauseUI.gameObject.SetActive(false);
        freeCameraButton.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }
}