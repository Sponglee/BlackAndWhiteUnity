using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRig : MonoBehaviour
{
    public CinemachineVirtualCamera[] rigCams;


    private void Awake()
    {
        transform.SetParent(CameraManager.Instance.transform);

        foreach (CinemachineVirtualCamera item in rigCams)
        {
            CameraManager.Instance.AddCamera(item);
        }
    }
}
