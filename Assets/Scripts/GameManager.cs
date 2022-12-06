using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    bool gameHasEnded = false;

    public float restartDelay = 1f;

    public GameObject completeLevelUI;
    public GameObject failLevelUI;
    public GameObject AHealthBar;
    public GameObject BHealthBar;
    
    void Awake()
    {
        if(Instance != null) 
        {
            Destroy(Instance);
        } 
        Instance = this;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void FailLevel(){
        if(gameHasEnded) return;
        gameHasEnded = true;
        Debug.Log("Game Lost!");
        failLevelUI.SetActive(true);
        AHealthBar.SetActive(false);
        BHealthBar.SetActive(false);
        // Application.Quit();
    }
    public void CompleteLevel(){
        if(gameHasEnded) return;
        gameHasEnded = true;
        Debug.Log("Game Won!");
        completeLevelUI.SetActive(true);
        AHealthBar.SetActive(false);
        BHealthBar.SetActive(false);
        // Application.Quit();
    }
}
