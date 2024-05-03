using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnenmyAttack : MonoBehaviour
{
    private BoxCollider handCollider;
    // Start is called before the first frame update
    void Start()
    {
        handCollider = GetComponentInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
    }
    private void OnTriggerEnter(Collider handCollider)
    {
        if (!GameManager.instance.isLive)
            return;

        if (handCollider.gameObject.CompareTag("Player"))
        {
            Player playerComponent = handCollider.gameObject.GetComponent<Player>();

            if (playerComponent != null)
            {
                if (playerComponent.playerCurrentHP <= 0)
                {
                    GameManager.instance.isLive = false;
                    Invoke("EndScene", 3f);
                }
                if (playerComponent.isInvincible == false)
                {
                    Debug.Log("Hit");
                    playerComponent.playerCurrentHP -= 1;
                }
            }
        }
    }
    public void EndScene()
    {
        SceneManager.LoadScene(2);
    }
}
