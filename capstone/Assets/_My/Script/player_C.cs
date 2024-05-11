using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class player_C : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera PlayerFollowCamera;
    [SerializeField] CinemachineVirtualCamera DeadCam;

    private Player player; // Player 클래스에 대한 참조
    private void Start()
    {
        // Player 오브젝트에 붙어있는 Player 클래스를 찾아서 참조를 가져옴
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        CameraSwitcher.Register(PlayerFollowCamera);
        CameraSwitcher.Register(DeadCam);
        CameraSwitcher.SwitchCamera(PlayerFollowCamera);
    }

    private void OnDisEnable()
    {
        CameraSwitcher.Register(PlayerFollowCamera);
        CameraSwitcher.Register(DeadCam);
    }
    private void Update()
    {
        if (player != null && player.playerCurrentHP <= 0)
        {
            if (CameraSwitcher.isActiveCamera(PlayerFollowCamera))
            {
                CameraSwitcher.SwitchCamera(DeadCam);
            }
            Debug.Log("전환이 완료되었씁니다,");
        }
    }
}

