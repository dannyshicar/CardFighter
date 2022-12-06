using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int round = 1;
    bool gameHasEnded = false;

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
    }

    void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }

    public void BeginGame(){
        Time.timeScale = 1f;
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
        round = 1;
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
            GameRoundCountText.text = round.ToString();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }
        gameHasEnded = true;
        Debug.Log("Game Won!");
        round = 1;
        completeLevelUI.SetActive(true);
        AHealthBar.SetActive(false);
        BHealthBar.SetActive(false);
        GameRoundCountUI.SetActive(false);
        skillBarUI.SetActive(false);
        // Application.Quit();
    }
}
