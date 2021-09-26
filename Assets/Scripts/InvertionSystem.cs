using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class InvertionSystem : MonoBehaviour
{
    [SerializeField] VolumeProfile normal;
    [SerializeField] VolumeProfile inverted;

    public Volume globalVolume;

    private void Start()
    {
        globalVolume.profile = normal;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleVolume();
        }
    }
    public void ToggleVolume()
    {
        globalVolume.profile = globalVolume.profile == normal ? inverted : normal;
    }

}
