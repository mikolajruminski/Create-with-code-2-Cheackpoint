 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  //audio source and sounds

   public AudioSource src;
   public List<AudioClip> clips = new List<AudioClip>();
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
    private float timeLimit = 60;

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
        restartScreen.gameObject.SetActive(false);
        
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
        StartCoroutine(loadRestart());
      }

    }
    
    //Lerping menus into existence 
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
    
    IEnumerator loadRestart () {
      float x = 0f;
      while (x < 5)
    {
      restartScreenAlpha.alpha = Mathf.Lerp(startValue, endValue, x / lerpDuration);
      x += Time.deltaTime;
      yield return null;
    } 
      restartScreenAlpha.alpha = endValue;
    }
    
    void getCanvas () {
      mainMenuAlpha = GameObject.Find("MenuScreen").GetComponent<CanvasGroup>();
      restartScreenAlpha = GameObject.Find("RestartScreen").GetComponent<CanvasGroup>();
    }

    //sounds sources 

    public void MainMenuButton() {
      src.clip = clips[0];
      src.Play();
    }

    public void PointSound() {
      src.clip = clips[1];
      src.Play();
    }

    public void BallRelease() {
      src.clip = clips[2];
      src.volume = 0.5f;
      src.Play();
    }

    public void BallTightening() {
      src.clip = clips[3];
      src.Play();
    }
}
