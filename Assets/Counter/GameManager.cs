using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    //audio source and sounds

    public AudioSource src;
    public AudioSource music;
    public List<AudioClip> clips = new List<AudioClip>();
    //Menus
    public Image pauseScreen;
    public GameObject gameMenu;

    public GameObject gameScreen;

    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI highScore;
    public TMP_InputField yourName;


    public GameObject restartScreen;

    private GameObject mainCamera;

    //canvas for menus alpha

    private CanvasGroup mainMenuAlpha;
    private CanvasGroup restartScreenAlpha;
    private CanvasGroup gameScreenAlpha;

    //

    private Vector3 cameraGamePosition = new Vector3(-5.78f, 6.52f, 28.66f);
    private Vector3 cameraGameRotation = new Vector3(0, 171.598f, 0f);
    private bool isPaused;
    public GameObject ball;
    public GameObject count;
    private Counter counterScript;
    public GameObject spawnPoint;
    public Vector3 spawnPointLocation;
    public bool isGameActive = false;
    bool isSpawning = false;
    private float timeLimit = 60;
    private string input;

    //Lerp values

    float timeElapsed;
    float startValue = 0;
    float endValue = 1;

    // Start is called before the first frame update
    void Awake()
    {

        getCanvas();
        loadData();
        yourName.text = input;

    }
    void Start()
    {
        counterScript = count.GetComponent<Counter>();
        mainCamera = GameObject.Find("Main Camera");
        isPaused = false;
        spawnPointLocation = spawnPoint.gameObject.transform.position;
        StartCoroutine(loadMenus(mainMenuAlpha, 1.5f));
        restartScreen.gameObject.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        if (counterScript.isDunked == true && isSpawning == false && isGameActive)
        {
            StartCoroutine(spawnBalls());
        }
        pauseGame();

        if (isGameActive == true)
        {
            timeLimiter();
        }
    }
    //spawning new balls
    IEnumerator spawnBalls()
    {
        isSpawning = true;
        Instantiate(ball, spawnPointLocation, ball.gameObject.transform.rotation);
        yield return new WaitForSeconds(1);
        counterScript.isDunked = false;
        isSpawning = false;
    }

    //game functions, start/pause/restart
    void pauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P) && isPaused == false && isGameActive)
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseScreen.gameObject.SetActive(true);
            music.Pause();
        }

        else if (Input.GetKeyDown(KeyCode.P) && isPaused == true && isGameActive)
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseScreen.gameObject.SetActive(false);
            music.UnPause();
        }
    }

    public void StartGame()
    {
        gameMenu.gameObject.SetActive(false);
        StartCoroutine(cameraToGamePosition(Quaternion.Euler(cameraGameRotation), 2f));
    }

    private void timeLimiter()
    {
        timeLimit -= Time.deltaTime;
        timeCounter.text = "Time Left : " + timeLimit.ToString("0") + " sec";

        if (timeLimit < 0)
        {
            isGameActive = false;
            restartScreen.gameObject.SetActive(true);
            StartCoroutine(loadMenus(restartScreenAlpha, 3f));
            StartCoroutine(deLoadMenus(gameScreenAlpha, 1f, 0.3f));
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void getCanvas()
    {
        mainMenuAlpha = GameObject.Find("MenuScreen").GetComponent<CanvasGroup>();
        restartScreenAlpha = GameObject.Find("RestartScreen").GetComponent<CanvasGroup>();
        gameScreenAlpha = GameObject.Find("GameScreen").GetComponent<CanvasGroup>();
    }

    //sounds sources 

    public void MainMenuButton()
    {
        src.clip = clips[0];
        src.Play();
    }

    public void PlaySound(int x, float volume)
    {
        src.clip = clips[x];
        src.Play();
    }


    //animation Lerping
    IEnumerator cameraToGamePosition(Quaternion endValue, float duration)
    {
        float y = 0;
        Quaternion startValue = mainCamera.transform.rotation;
        while (y < duration)
        {
            mainCamera.transform.rotation = Quaternion.Lerp(startValue, endValue, y / 0.66f);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraGamePosition, y / 27);
            y += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = cameraGamePosition;
        mainCamera.transform.rotation = endValue;
        StartCoroutine(loadMenus(gameScreenAlpha, 0.5f));
        yield return new WaitForSeconds(1);
        gameScreen.gameObject.SetActive(true);
        isGameActive = true;
    }

    IEnumerator loadMenus(CanvasGroup toLoad, float duration)
    {
        float x = 0f;
        while (x < duration)
        {
            toLoad.alpha = Mathf.Lerp(startValue, endValue, x / duration);
            x += Time.deltaTime;
            yield return null;
        }
        toLoad.alpha = endValue;
    }

    IEnumerator deLoadMenus(CanvasGroup toLoad, float duration, float differentStartValue)
    {
        float x = 0f;
        while (x < duration)
        {
            toLoad.alpha = Mathf.Lerp(endValue, differentStartValue, x / duration);
            x += Time.deltaTime;
            yield return null;
        }
        toLoad.alpha = differentStartValue;
    }

    //get input from the player 
    public void ReadStringInput(string s)
    {
        input = s;
    }

    //save name and high score between sessions

    [System.Serializable]
    public class PlayerData
    {
        public string name;
        public int highScore;
    }

    public void saveData()
    {
        PlayerData data = new PlayerData();
        data.name = input;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void loadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            input = data.name;
        }

    }

    private void OnApplicationQuit()
    {
        saveData();
    }

}
