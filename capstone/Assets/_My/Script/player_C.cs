using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class player_C : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera PlayerFollowCamera;
    [SerializeField] CinemachineVirtualCamera DeadCam;

    private Player player; // Player Ŭ������ ���� ����
    private void Start()
    {
        // Player ������Ʈ�� �پ��ִ� Player Ŭ������ ã�Ƽ� ������ ������
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
            Debug.Log("��ȯ�� �Ϸ�Ǿ����ϴ�,");
        }
    }
}

