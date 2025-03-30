using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{

    public Button miniGameOne;
    public Button miniGameTwo;
    public Button Back;

    public TextMeshProUGUI miniGameChoose;


    public GameObject miniGames;
    public GameObject startMenus;
    void Start()
    {
        
        GameObject startButton = GameObject.Find("StartButton");
        GameObject upgradeButton = GameObject.Find("UpgradeButton");
        GameObject challengeButton = GameObject.Find("Challenge");
        GameObject minigameButton = GameObject.Find("MinigameButton");
        GameObject exitButton = GameObject.Find("ExitButton");
        



        if (startButton != null)
        {
            startButton.GetComponent<Button>().onClick.AddListener(OnStartButtonClick);
        }
        if (upgradeButton != null)
        {
            upgradeButton.GetComponent<Button>().onClick.AddListener(OnUpgradeButtonClick);
        }
        if (challengeButton != null)
        {
            challengeButton.GetComponent<Button>().onClick.AddListener(OnChallengeButtonClick);
        }
        if (minigameButton != null)
        {
            minigameButton.GetComponent<Button>().onClick.AddListener(OnMinigameButtonClick);
        }
        if (exitButton != null)
        {
            exitButton.GetComponent<Button>().onClick.AddListener(OnExitButtonClick);
        }
        if (miniGameOne != null)
        {
            miniGameOne.GetComponent<Button>().onClick.AddListener(MiniGameOne);
        }
        if (miniGameTwo != null)
        {
            miniGameTwo.GetComponent<Button>().onClick.AddListener(MiniGameTwo);
        }
        if (miniGameOne != null)
        {
            Back.GetComponent<Button>().onClick.AddListener(BackToMain);
        }

    }

    void OnStartButtonClick()
    {
        SceneManager.LoadScene("CharacterChoice");
    }

    void OnUpgradeButtonClick()
    {
        SceneManager.LoadScene("Ability");
    }

     void OnChallengeButtonClick()
    {
        SceneManager.LoadScene("Challenge");
    }


    void OnMinigameButtonClick()
    {
        startMenus.SetActive(false);
        miniGames.SetActive(true); 
        Debug.Log("Minigame 버튼 클릭됨. 나중에 씬을 추가하세요.");
    }

    void OnExitButtonClick()
    {
        Application.Quit();
    }


    void MiniGameOne()
    {
        SceneManager.LoadScene("MiniGame1");
    }
    void MiniGameTwo()
    {
        SceneManager.LoadScene("MiniGame2");
    }


    void BackToMain()
    {
        miniGames.SetActive(false);
        startMenus.SetActive(true);
    }
}
