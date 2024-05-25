using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private CapsuleCollider healCollider;
    // Start is called before the first frame update
    void Start()
    {
        healCollider = GetComponentInChildren<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
    }

    private void OnTriggerEnter(Collider healCollider)
    {
        if (!GameManager.instance.isLive)
            return;

        if (healCollider.gameObject.CompareTag("Player"))
        {
            Player playerComponent = healCollider.gameObject.GetComponent<Player>();

            if (playerComponent != null && playerComponent.playerCurrentHP <= 9)
            {
                if (playerComponent.playerCurrentHP <= 0)
                {
                    GameManager.instance.isLive = false;
                }
                if (playerComponent.isInvincible == false)
                {
                    playerComponent.playerCurrentHP = Mathf.Min(playerComponent.playerCurrentHP + 3, 10);
                    Destroy(gameObject);
                }
            }
        }
    }
}
