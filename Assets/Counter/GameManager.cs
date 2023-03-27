 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Image pauseScreen;
    public GameObject gameMenu;

    public GameObject gameScreen;

    public TextMeshProUGUI timeCounter;

    public GameObject restartScreen;
    
    private bool isPaused;
    public GameObject ball;
    public GameObject count;
    private Counter counterScript;
    public Vector3 spawnPoint = new Vector3(-7.02f, 4.76f, 10.94301f);
    public bool isGameActive = false;
    bool isSpawning = false;
    private float timeLimit = 50;
    // Start is called before the first frame update
    void Start()
    {
        counterScript = count.GetComponent<Counter>();
        isPaused = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counterScript.isDunked == true && isSpawning == false && isGameActive) {
          StartCoroutine(spawnBalls());
        }
        pauseGame();

        if (isGameActive == true) {
            timeLimiter();
        }
    }

    IEnumerator spawnBalls () {
        isSpawning = true;
       Instantiate(ball, spawnPoint, ball.gameObject.transform.rotation);
       yield return new WaitForSeconds(1);
       counterScript.isDunked = false;
       isSpawning = false;
    }

    void pauseGame () {
        if (Input.GetKeyDown(KeyCode.P) && isPaused == false && isGameActive) {
            Time.timeScale = 0;
            isPaused = true;
            pauseScreen.gameObject.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.P) && isPaused == true && isGameActive) {
            isPaused = false;
            Time.timeScale = 1;
            pauseScreen.gameObject.SetActive(false);
        }
    }

     public void StartGame () {

     gameMenu.gameObject.SetActive(false);
     isGameActive = true;
     gameScreen.gameObject.SetActive(true);
     Instantiate(ball, spawnPoint, ball.gameObject.transform.rotation);
    }

    private void timeLimiter () {
      timeLimit -= Time.deltaTime;
      timeCounter.text = "Time Left : " + timeLimit.ToString("0") + " sec";

      if (timeLimit < 0) {
        isGameActive = false;
        restartScreen.gameObject.SetActive(true);
      }

    }

    public void RestartGame() {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
