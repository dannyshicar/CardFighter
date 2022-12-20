using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // round settings
    public static int round = 1;
    public static int bossHP = 100;

    bool gameHasEnded = false;
    public static bool gameHasStarted = false;

    public float restartDelay = 1f;

    public GameObject completeLevelUI;
    public GameObject failLevelUI;
    public GameObject AHealthBar;
    public GameObject BHealthBar;
    public GameObject GameRoundCountUI;
    public GameObject skillBarUI;
    // public GameObject GameRoundCountUI;
    public TMPro.TextMeshProUGUI GameRoundCountText;
    
    void Awake()
    {
        if(Instance != null) 
        {
            Destroy(Instance);
        } 
        Instance = this;
        GameRoundCountText.text = round.ToString();
        Time.timeScale = 0f;
        gameHasStarted = false;
        gameHasEnded = false;
    }

    void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    void resetRoundSettings()
    {
        round = 1;
        bossHP = 100;
    }

    public void BeginGame(){
        Time.timeScale = 1f;
        gameHasStarted = true;
    }
    
    public void EndGame(){
        if (gameHasEnded == false){

            gameHasEnded = true;
            Debug.Log("Game Over");
            //restart game
            Invoke("Restart", restartDelay);
        }
    }
    void Restart(){
        round = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FailLevel(){
        if(gameHasEnded) return;
        gameHasEnded = true;
        Debug.Log("Game Lost!");
        resetRoundSettings();
        failLevelUI.SetActive(true);
        AHealthBar.SetActive(false);
        BHealthBar.SetActive(false);
        GameRoundCountUI.SetActive(false);
        skillBarUI.SetActive(false);
        // Application.Quit();
    }
    public void CompleteLevel(){
        if(gameHasEnded) return;
        if(round < 3){
            round++;
            bossHP *= 2;
            GameRoundCountText.text = round.ToString();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        gameHasEnded = true;
        Debug.Log("Game Won!");
        resetRoundSettings();
        completeLevelUI.SetActive(true);
        AHealthBar.SetActive(false);
        BHealthBar.SetActive(false);
        GameRoundCountUI.SetActive(false);
        skillBarUI.SetActive(false);
        // Application.Quit();
    }
}
