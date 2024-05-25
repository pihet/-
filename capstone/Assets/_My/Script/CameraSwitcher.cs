using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public static class CameraSwitcher 
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    public static CinemachineVirtualCamera ActiveCamera = null;

    public static bool isActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == ActiveCamera;
    }

    public static void SwitchCamera(CinemachineVirtualCamera Camera)
    {
        Camera.Priority = 10;
        ActiveCamera = Camera;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c != Camera && c.Priority !=0)
            {
                c.Priority = 0;
            }
        }
    }
    public static void Register(CinemachineVirtualCamera Camera)
    {
        cameras.Add(Camera);
    }
    public static void UnRegister(CinemachineVirtualCamera Camera)
    {
        cameras.Remove(Camera);
    }

}
