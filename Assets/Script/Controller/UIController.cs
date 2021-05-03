using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BasketballGameDefinition;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject topPanel;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Text bestScoreTxt;
    [SerializeField] private GameObject balls;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private Text levelTxt;
    [SerializeField] private Text targetTxt;

    [SerializeField] private GameObject stopPanel;
    [SerializeField] private GameObject failedPanel;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private Text newLevelTxt;
    [SerializeField] private Text newTargetTxt;

    [SerializeField] private GameObject endPanel;

    public static UIController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    // oyuncuya baþlangýç hedefi göster
    public void SetStartPanel(int _level,int _targetScore)
    {
        levelTxt.text = "Level : "+ (_level + 1).ToString();
        targetTxt.text = "Target Score : "+_targetScore.ToString();
    }

    // oyuncu baþarýlý ise yeni hedefi göster
    public void SetSuccessPanel(int _level, int _targetScore)
    {
        newLevelTxt.text = "Next Level : " + (_level+1).ToString();
        newTargetTxt.text = "Target Score : " + _targetScore.ToString();
    }

    // top paneldeki oyuncu scoru
    public void SetScore(int _score)
    {
        scoreTxt.text = "Score :"+_score.ToString();
    }

    // top paneldeki best score
    public void SetBestScore(int _bestScore)
    {
        bestScoreTxt.text = "Best Score : "+_bestScore.ToString();
    }

    // top paneldeki toplar
    public void ResetBalls(int _ball=0)
    {
        for (int i = 0; i < balls.transform.childCount; i++)
        {
            balls.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < _ball; i++)
        {
            balls.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
       
    // kalan top sayýsýný göster
    public void HideBall(int _index)
    {
        balls.transform.GetChild(_index).gameObject.SetActive(false);
    }

    // istenilen paneli aç
    public void OpenSelectedPanel(Panels _panel)
    {
        switch (_panel)
        {
            case Panels.StartPanel:
                startPanel.SetActive(true);
                break;
            case Panels.FailedPanel:
                stopPanel.SetActive(true);
                failedPanel.SetActive(true);
                break;
            case Panels.SuccessPanel:
                stopPanel.SetActive(true);
                successPanel.SetActive(true);
                break;
            case Panels.EndPanel:
                endPanel.SetActive(true);
                break;
            default:                
                break;
        }
    }


}
