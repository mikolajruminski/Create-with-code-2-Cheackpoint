using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowBalls : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody ballRB;
    private GameObject ground;
    private GameManager gameManager;
    private Vector3 firstPos;
    private Vector3 secondPos;

    private Vector3 maxThrowVec = new Vector3(12f, 6f, 2f);
    private Vector3 throwVec;
    private Rigidbody rb;
   private RaycastHit hit;
   public GameObject trail;
   
   public bool isGrounded;
   public bool isActive;

    Camera cam;

    private void Start() {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        isActive = true;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isGrounded && gameManager.isGameActive){
            firstPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 23f));
            gameManager.BallTightening();
            
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//
             // if (Physics.Raycast (ray, out hit, 100.0f)) {
             // GameObjectClicked = hit.collider.gameObject.GetComponent<Rigidbody>();
              //GameObjectClicked.AddForce(new Vector3(0,10,10) * 0.2f, ForceMode.Impulse);
         // } 
        }

        if (Input.GetMouseButtonUp(0) && isGrounded && gameManager.isGameActive) {
            secondPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 23f));
            throwVec = firstPos - secondPos;
            if (isActive == true) {
               gameManager.BallRelease();
               rb.AddForce(throwVec * 1f, ForceMode.Impulse);
               gameObject.GetComponent<TrailRenderer>().emitting = true;
            }
            isGrounded = false;
        }
        if (Input.GetKey(KeyCode.R) && gameManager.isGameActive && isActive){
            StartCoroutine(restartPosition());
        }
        restartPosition();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sensor")) {
            isActive = false;
            gameObject.GetComponent<TrailRenderer>().emitting = false;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")){
            Debug.Log("Touching ground!");
            isGrounded = true;
            gameObject.GetComponent<TrailRenderer>().emitting = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")){
            Debug.Log("Not Touching ground!");
        }
    }
    
    
    IEnumerator restartPosition () {
            gameObject.GetComponent<TrailRenderer>().emitting = false;
            transform.position = gameManager.spawnPointLocation;
            yield return null;
            gameObject.GetComponent<TrailRenderer>().emitting = true;

    }

}
