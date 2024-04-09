using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs input;

    [Header("Aim")]
    [SerializeField]
    private CinemachineVirtualCamera aimCam;
    [SerializeField]
    private GameObject aimImage;
    [SerializeField]
    private LayerMask targetLayer;

    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        AimCheck();
    }

    private void AimCheck()
    {
        if(input.aim)
        {
            aimCam.gameObject.SetActive(true);
            aimImage.SetActive(true);

            Vector3 targetPosition = Vector3.zero;
            Transform camTransform = Camera.main.transform; //카메라의 위치 값 받아오기
            RaycastHit hit; // 대상 콜라이더에 충돌된 정보 받아오기

            if(Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
            {
                //Debug.Log("Name : " + hit.transform.gameObject.name);
                targetPosition = hit.point;
            }
            else
            {
                // 만약 레이캐스트가 뭔가를 맞히지 않았다면, 카메라의 정면 방향을 조준 방향으로 설정
                targetPosition = camTransform.position + camTransform.forward * 100f; // 카메라의 정면 방향으로 조준
            }

            Vector3 targetAim = targetPosition;
            targetAim.y = transform.position.y;

            // 조준하고 있는 방향으로 회전
            Vector3 aimDir = (targetAim - transform.position).normalized;
            transform.forward = aimDir;
        }
        else
        {
            aimCam.gameObject.SetActive(false);
            aimImage.SetActive(false);
        }
    }
}