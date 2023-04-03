using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicDontDestroy : MonoBehaviour
{
    private static musicDontDestroy instance;
    // Start is called before the first frame update

    void Awake() 
    {
      if (instance != null && instance != this)
         {
             Destroy(this.gameObject);
             return;
         }
         else
         {
             instance = this;
         }
         DontDestroyOnLoad(this.gameObject);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
