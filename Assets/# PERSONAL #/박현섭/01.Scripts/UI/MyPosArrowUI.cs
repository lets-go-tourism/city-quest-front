using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Todo : 
// �÷��̾��� ���� ��ġ�� ����Ű�� ȭ��ǥ�� ������

public class MyPosArrowUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float offsetY;
    [SerializeField] private float radius = 10f;

    private RectTransform rectTransform;
    private Camera cam;
    private Image myImage;



    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        myImage = GetComponent<Image>();
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 targetPos = GPS.Instance.GetUserWorldPosition();

        if (VisibleInScreen(targetPos) == false) 
        {
            myImage.enabled = true;

            Vector3 vector3 = targetPos - cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, offsetY + Screen.height / 2, cam.transform.position.y + 1));

            Vector2 atanVector = new Vector2(vector3.x, vector3.z);
            Vector2 dir = atanVector.normalized * radius;

            float angle = Mathf.Atan2(atanVector.y, atanVector.x) * Mathf.Rad2Deg - 90;

            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
            rectTransform.anchoredPosition = new Vector2(0, offsetY) + dir;
        }
        else
        {
            myImage.enabled = false;
        }
    }

    private bool VisibleInScreen(Vector3 targetPos)
    {
        // ȭ���� ���ϴ�, ������ ������ǥ�� ���
        Vector3 leftDownDot = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.transform.position.y + 1));
        Vector3 rightUpDot = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.y + 1));

        // ȭ�� �ȿ� ������Ʈ�� ���� ������ true �ƴϸ� false�� ��ȯ
        if (leftDownDot.x < targetPos.x && rightUpDot.x > targetPos.x && leftDownDot.z < targetPos.z && rightUpDot.z > targetPos.z)
            return true;
        else
            return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MapCameraController.Instance.StartCameraMoveToTarget(GPS.Instance.GetUserWorldPosition());
    }
}
