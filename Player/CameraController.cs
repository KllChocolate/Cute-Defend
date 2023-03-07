using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class CameraController : MonoBehaviour
{
    [SerializeField] public bool freeLook = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !freeLook)
        {
            GetComponent<CinemachineFreeLook>().enabled = false;
            freeLook = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && freeLook)
        {
            GetComponent<CinemachineFreeLook>().enabled = true;
            freeLook = false;
        }
    }
}
