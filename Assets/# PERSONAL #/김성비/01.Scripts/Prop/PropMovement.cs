using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropMovement : MonoBehaviour
{
    private void Start()
    {
        point = new PointerEventData(null);
        results = new List<RaycastResult>();
    }

    void Update()
    {
        RayTouch();
    }
    List<RaycastResult> results;
    public GraphicRaycaster raycaster;
    PointerEventData point;
    Touch touch;

    public bool spin { get; private set; } = false;

    float originX;
    float moveX;
    float diffX;
    float modelY;

    int count;
    public GameObject[] modelings;
    GameObject realModeling;

    public void SettingModeling()
    {
        if (count == 0)
        {
            for (int i = 0; i < modelings.Length; i++)
            {
                if (modelings[i].activeSelf == false) continue;
                realModeling = modelings[i];
                print(realModeling.name);
            }
            count++;
        }
    }

    void RayTouch()
    {
        if (PopUpMovement.instance.placeState == PopUpMovement.PlaceState.UP)
        {
            //point = GameObject.Find()
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    results = new List<RaycastResult>();
                    point.position = touch.position;
                    raycaster.Raycast(point, results);

                    foreach (RaycastResult r in results)
                    {
                        if (r.gameObject.CompareTag("Modeling"))
                        {
                            spin = true;
                            originX = touch.position.x;
                            modelY = realModeling.transform.rotation.eulerAngles.y;
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved && spin)
                {
                    moveX = touch.position.x;
                    diffX = modelY + (originX - moveX);
                    realModeling.transform.localRotation = Quaternion.Euler(0, diffX, 0);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    count = 0;
                    diffX = 0;
                    moveX = 0;
                    originX = 0;
                    spin = false;
                    results = null;
                }

            }
        }
    }
}
