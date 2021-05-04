using BasketballGameDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Cloth clothObject;
    private AudioSource auSource;


    // level datalarý
    private LevelDesignScriptableObject levelObject;
    string levelDesignObjectPath = "Levels"+Path.DirectorySeparatorChar;

    // player datalarý
    private string checkPrefKey;
    private int TotalLevel = 10;
    private int level;
    private int bestScore;
    private int score;
    // oyun datalarý
    private int ballCount;
    private int numberOfBallsThrown;
    private int successCriterion;

    public static GameManager Instance;
    public static Action Basket;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        CheckPlayerData();

        Basket = ThrowBasket;
        auSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        CreateLevel();
        UIController.Instance.OpenSelectedPanel(Panels.StartPanel);
        UIController.Instance.SetStartPanel(level, successCriterion);
    }

    private void OnDestroy()
    {
        Basket = null;
    }

    // level için gerekli bilgileri al
    private void CreateLevel()
    {
        levelObject = Resources.Load<LevelDesignScriptableObject>(levelDesignObjectPath + level.ToString());

        if(levelObject == null)
        {
            Debug.Log("level objesi yok, oyun bitmiþ");
            UIController.Instance.OpenSelectedPanel(Panels.EndPanel);
            return;
        }

        RenderSettings.skybox = levelObject.skyboxMaterial;
        player.transform.position = levelObject.playerPosition;
        player.transform.eulerAngles = levelObject.playerRotation;
        ballCount = levelObject.BallCount;
        successCriterion = levelObject.SuccessCount;
    }
    
    // topu level e göre özelleþtir
    /*
    private Transform CustomizedBallTransform()
    {
        Transform _ball = ObjectPooling.Instance.GetPooledObject(0).transform;      
        _ball.eulerAngles = Vector3.zero;
        _ball.localScale = Vector3.one * levelObject.Ballsize;
        _ball.position = levelObject.BallStartPosition;
        _ball.GetComponent<ShootBall>().speed = levelObject.BallSpeed;
        _ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _ball.GetComponent<Rigidbody>().mass = levelObject.Ballmass;
        _ball.GetComponent<ShootBall>().SetMaterialColor(levelObject.Ballcolor);

        return _ball;
    }
    */


    // kayýtlý kullanýcý bilgilerini al
    private void CheckPlayerData()
    {
        checkPrefKey = PlayerPrefKey.PlayerBestScore.ToString();
        if (PlayerPrefs.HasKey(checkPrefKey))
        {
            int _value = PlayerPrefs.GetInt(checkPrefKey);

            bestScore = _value;
        }
        else
        {
            bestScore = 0;
        }

        checkPrefKey = PlayerPrefKey.PlayerLevel.ToString();
        if (PlayerPrefs.HasKey(checkPrefKey))
        {
            int _value = PlayerPrefs.GetInt(checkPrefKey);

            level = _value;
        }
        else
        {
            level = 0;
        }
    }

    // oyunu oluþtur
    private IEnumerator CreateGame()
    { 
        yield return new WaitUntil(()=> ObjectPooling.Instance.isPooled);
        UIController.Instance.ResetBalls(ballCount);
        //CustomizedBallTransform();
        CustomizeAllBall();
        ObjectPooling.Instance.GetPooledObject(0);
    }

    // top atýldý
    public void BallShooted()
    {
        UIController.Instance.HideBall(numberOfBallsThrown);
        numberOfBallsThrown++;        

        if (numberOfBallsThrown < ballCount)
            StartCoroutine(GetNewBall());
        else
            StartCoroutine(CheckSuccess());

    }
   
    // skor elde etti
    private void ThrowBasket()
    {
        score++;
        UIController.Instance.SetScore(score);

        if (score > bestScore)
        {
            bestScore = score;
            UIController.Instance.SetBestScore(bestScore);
            checkPrefKey = PlayerPrefKey.PlayerBestScore.ToString();
            PlayerPrefs.SetInt(checkPrefKey,bestScore);
        }

    }

    // levelin baþarý kontrolü
    private void CheckPlayerSuccess()
    {
        if (score >= successCriterion)
        {
            AudioController.Instance.PlaySound(SoundEffect.SuccessLevel,auSource);
            level++;
            CreateLevel();
            PlayerPrefs.SetInt(PlayerPrefKey.PlayerLevel.ToString(), level);
            UIController.Instance.OpenSelectedPanel(Panels.SuccessPanel);
            UIController.Instance.SetSuccessPanel(level, successCriterion);
        }
        else
        {
            AudioController.Instance.PlaySound(SoundEffect.FailedLevel, auSource);
            UIController.Instance.OpenSelectedPanel(Panels.FailedPanel);
        }
            
    }

    // yeni top getir
    IEnumerator GetNewBall()
    {
        yield return new WaitForSeconds(1f);
        //CustomizedBallTransform();
        Transform _ball= ObjectPooling.Instance.GetPooledObject(0).transform;
        ResetBall(_ball);
    }

    // bak bakalým hedefe ulaþmýþ mýyýz
    IEnumerator CheckSuccess()
    {
        yield return new WaitForSeconds(3f);
        CheckPlayerSuccess();
    }

    // oyun Baþlasýn
    public void StartGame()
    {
        UIController.Instance.SetBestScore(bestScore);
        StartCoroutine(CreateGame());
    }

    // yeniden oyna
    public void Replay()
    {
        score = 0;
        numberOfBallsThrown = 0;
        UIController.Instance.SetScore(score);
        StartCoroutine(CreateGame());
    }

    // yeni leveli oyna
    public void PlayNextLevel()
    {
        score = 0;
        numberOfBallsThrown = 0;
        UIController.Instance.SetScore(score);
        StartCoroutine(CreateGame());
    }

    // tüm oyunu sýfýrla
    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();

        score = 0;
        numberOfBallsThrown = 0;
        UIController.Instance.SetScore(score);
        Transform _poolParent = ObjectPooling.Instance.pool[0].parent;
        for (int i = 0; i < _poolParent.childCount; i++)
        {
            _poolParent.GetChild(i).gameObject.SetActive(false);
        }

        CheckPlayerData();

        CreateLevel();
        UIController.Instance.OpenSelectedPanel(Panels.StartPanel);
        UIController.Instance.SetStartPanel(level, successCriterion);

    }


    // tüm toplarý özelleþtir
    private void CustomizeAllBall()
    {     
        List<ClothSphereColliderPair> _clothColliders = new List<ClothSphereColliderPair>();

        for (int i = 0; i < ObjectPooling.Instance.pool[0].parent.childCount; i++)
        {
            Transform _ball = ObjectPooling.Instance.pool[0].parent.GetChild(i);
            ResetBall(_ball);
            _ball.GetComponent<ShootBall>().speed = levelObject.BallSpeed;            
            _ball.GetComponent<ShootBall>().SetMaterialColor(levelObject.Ballcolor);

            _clothColliders.Add((new ClothSphereColliderPair(_ball.GetComponent<SphereCollider>())));

           _ball.gameObject.SetActive(false);
        }

        clothObject.sphereColliders = _clothColliders.ToArray();
        _clothColliders.Clear();
    }

    // topu resetle
    private void ResetBall(Transform _ball)
    {
        _ball.eulerAngles = Vector3.zero;
        _ball.localScale = Vector3.one * levelObject.Ballsize;
        _ball.position = levelObject.BallStartPosition;
        _ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        _ball.GetComponent<Rigidbody>().mass = levelObject.Ballmass;
    }
}
