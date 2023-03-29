 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  //Menus
    public Image pauseScreen;
    public GameObject gameMenu;

    public GameObject gameScreen;

    public TextMeshProUGUI timeCounter;

    public GameObject restartScreen;

    //canvas for menus alpha

    private CanvasGroup mainMenuAlpha;
    private CanvasGroup restartScreenAlpha;
    private CanvasGroup gameScreenAlpha;
    
    //
    private bool isPaused;
    public GameObject ball;
    public GameObject count;
    private Counter counterScript;
    public GameObject spawnPoint;
    public Vector3 spawnPointLocation;
    public bool isGameActive = false;
    bool isSpawning = false;
    private float timeLimit = 50;

    //Lerp values

    float timeElapsed;
    float lerpDuration = 1.5f;
    float startValue=0;
    float endValue=1;

    // Start is called before the first frame update
    void Awake() {

     getCanvas();

    }
    void Start()
    {
        counterScript = count.GetComponent<Counter>();
        isPaused = false;
        spawnPointLocation = spawnPoint.gameObject.transform.position;
        StartCoroutine(loadMenu());
        
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
       Instantiate(ball, spawnPointLocation, ball.gameObject.transform.rotation);
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

    IEnumerator loadMenu () {
      while (timeElapsed < lerpDuration)
    {
      mainMenuAlpha.alpha = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
      timeElapsed += Time.deltaTime;
      yield return null;
    } 
      mainMenuAlpha.alpha = endValue;
    }
    
    void getCanvas () {
      mainMenuAlpha = GameObject.Find("MenuScreen").GetComponent<CanvasGroup>();
    }
}
