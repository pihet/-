using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnenmyAttack : MonoBehaviour
{
    public float damage;
    private BoxCollider handCollider;
    public Image bloodScreen;
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
                    //Debug.Log("Hit");
                    playerComponent.playerCurrentHP -= damage;
                    StartCoroutine(ShowBloodScreen());
                }
            }
        }
    }
    public void EndScene()
    {
        SceneManager.LoadScene(2);
    }

    IEnumerator ShowBloodScreen() {
        
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.5f);
        bloodScreen.color = Color.clear;
    }
}
