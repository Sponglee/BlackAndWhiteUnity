using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePivot : MonoBehaviour
{
    public Camera cam;
    public Vector3 pivotRange;
    public GameObject canvasPref;
    public PivotCanvas uiPivot;



    public GameObject pivot; // we will place this object at the mouse position
    void Start()
    {
        // gob = new GameObject();
        cam = Camera.main;


        uiPivot = Instantiate(canvasPref, GameObject.FindGameObjectWithTag("Canvases").transform).GetComponent<PivotCanvas>();


    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            // float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            // transform.rotation = Quaternion.Euler(0, rotation, 0);
            pivot.transform.position = target;

            //recalculate uiIndicator position
            uiPivot.UpdatePivotPosition(cam.WorldToScreenPoint(pivot.transform.position));

            pivot.transform.localPosition = new Vector3(Mathf.Clamp(pivot.transform.localPosition.x, -pivotRange.x, pivotRange.x),
                                                Mathf.Clamp(pivot.transform.localPosition.y, -pivotRange.y, pivotRange.y),
                                                Mathf.Clamp(pivot.transform.localPosition.z, -pivotRange.z, pivotRange.z));
        }

    }

}