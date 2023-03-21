 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pauseScreen;
    private bool isPaused;
    public GameObject ball;
    public GameObject count;
    private Counter counterScript;
    private Vector3 spawnPoint = new Vector3(-8.08f, 0.25f, -5.48f);

    bool isSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        counterScript = count.GetComponent<Counter>();
        isPaused = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (counterScript.isDunked == true && isSpawning == false) {
          StartCoroutine(spawnBalls());
        }
        pauseGame();
    }

    IEnumerator spawnBalls () {
        isSpawning = true;
       Instantiate(ball, spawnPoint, ball.gameObject.transform.rotation);
       yield return new WaitForSeconds(1);
       counterScript.isDunked = false;
       isSpawning = false;
    }

    void pauseGame () {
        if (Input.GetKeyDown(KeyCode.P) && isPaused == false) {
            Time.timeScale = 0;
            isPaused = true;
            pauseScreen.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.P) && isPaused == true) {
            isPaused = false;
            Time.timeScale = 1;
            pauseScreen.gameObject.SetActive(false);
        }
    }
}
