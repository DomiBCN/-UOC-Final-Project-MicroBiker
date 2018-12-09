using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelBtn : MonoBehaviour
{
    [Header("Level Unblocked")]
    public Button LevelBnt;
    public TextMeshProUGUI LevelNumber;
    public Image star1;
    public Image star2;
    public Image star3;

    [Header("Level Blocked")]
    public Transform levelBlcoked;
    
    [HideInInspector]
    public Level levelData;

    Color achievedStarColor;
    
    public void InitBtnData()
    {
        if (levelData.Unblocked)
        {
            SetUserStars();
            FillListener();
        }
        else
        {
            levelBlcoked.gameObject.SetActive(true);
        }
    }

    void SetUserStars()
    {
        achievedStarColor = Color.white;
        achievedStarColor.a = 1;

        LevelNumber.text = levelData.LevelId.ToString();
        if (levelData.MaxScore >= 10000)
        {
            star1.color = achievedStarColor;
        }

        if (levelData.MaxScore >= 20000)
        {
            star2.color = achievedStarColor;
        }

        if (levelData.MaxScore >= 30000)
        {
            star2.color = achievedStarColor;
        }
    }

    public void LoadLevel(int levelId)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelId);
        //SceneManager.LoadScene("LevelScene");
        SceneManager.LoadScene("TestingScene");
    }

    void FillListener()
    {
        LevelBnt.onClick.AddListener(() =>
        {
            LoadLevel(levelData.LevelId);
        });
    }
}
