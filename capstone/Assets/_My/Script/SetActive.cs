using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public GameObject victoryObject;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
        if(remainTime == 0){
            victoryObject.SetActive(true);
        }
    }
}
