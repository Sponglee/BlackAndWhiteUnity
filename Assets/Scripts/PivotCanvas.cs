using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotCanvas : MonoBehaviour
{
    public Transform pivotImage;

    public void UpdatePivotPosition(Vector3 pos)
    {
        pivotImage.position = pos;
    }
}
