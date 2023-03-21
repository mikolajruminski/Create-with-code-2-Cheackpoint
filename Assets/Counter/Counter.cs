using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    public Text CounterText;

    private int Count = 0;
    public bool isDunked = false;

    public GameObject ball;
    private ThrowBalls ballsScript;

    private void Start()
    {
        Count = 0;
        ballsScript = ball.GetComponent<ThrowBalls>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ScorePoint();
    }

    private void ScorePoint() 
    {
         Count += 1;
         CounterText.text = "Count : " + Count;
         isDunked = true;

    }
}
