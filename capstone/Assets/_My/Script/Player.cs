using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Slider HPSlider;

    private float playerMaxHP = 10;
    public float playerCurrentHP = 0;
    public float playerMinHP = 0;
    void Start()
    {
        InitPlayerHP();
    }

    // Update is called once per frame
    void Update()
    {
        HPSlider.value = playerCurrentHP / playerMaxHP;
    }

    private void InitPlayerHP()
    {
        playerCurrentHP = playerMaxHP;
    }
}
