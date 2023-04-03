using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Counter : MonoBehaviour
{
    public TextMeshProUGUI CounterText;
    private GameManager gameManager;
    private Vector3 fireworksPlace = new Vector3(0.59f, 8.25f, 10.82f);

    public ParticleSystem fireworks;

    public int Count = 0;
    public bool isDunked = false;

    public GameObject ball;
    private ThrowBalls ballsScript;

    private void Start()
    {
        isDunked = true;
        Count = 0;
        ballsScript = ball.GetComponent<ThrowBalls>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ScorePoint();
    }
   
   /* void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.GetContact(0).normal);
        if (other.gameObject.CompareTag("Sensor")) //include normals not to allow the ball to dunk from the bottom
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.collider);
            ScorePoint();
        }
        else {
            Physics.IgnoreCollision(GetComponent<Collider>(), other.collider);
        }
    }
    */

    private void ScorePoint() 
    {
         Count += 1;
         CounterText.text = "Score : " + Count;
         isDunked = true;
         StartCoroutine(fireworksPlay());
         gameManager.PlaySound(1, 0.7f);
    }

    IEnumerator fireworksPlay () {
     Instantiate(fireworks, fireworksPlace, transform.rotation);
     yield return new WaitForSeconds(1);
     Destroy(GameObject.FindGameObjectWithTag("Fireworks"));
    }
}
