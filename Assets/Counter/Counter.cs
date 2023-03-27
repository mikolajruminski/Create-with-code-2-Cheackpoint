using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public Text CounterText;
    public GameObject gam;

    private Vector3 fireworksPlace = new Vector3(0.59f, 8.25f, 10.82f);
    private AudioSource gameManager;

    public ParticleSystem fireworks;

    private int Count = 0;
    public bool isDunked = false;

    public GameObject ball;
    private ThrowBalls ballsScript;

    private void Start()
    {
        Count = 0;
        ballsScript = ball.GetComponent<ThrowBalls>();
        gameManager = gam.GetComponent<AudioSource>();
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
         gameManager.Play();
         StartCoroutine(fireworksPlay());
    }

    IEnumerator fireworksPlay () {
     Instantiate(fireworks, fireworksPlace, transform.rotation);
     yield return new WaitForSeconds(1);
     Destroy(GameObject.FindGameObjectWithTag("Fireworks"));
    }
}
