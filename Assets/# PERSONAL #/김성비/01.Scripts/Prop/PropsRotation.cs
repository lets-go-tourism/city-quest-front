using UnityEngine;

public class PropsRotation : MonoBehaviour
{
    public float rotateSpeed = 10.0f;

    // ���ڸ� ȸ��
    void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }

    // �巡�׷� ȸ��
    //public void OnDrag(PointerEventData eventData)
    //{
    //    float x = eventData.delta.x * rotateSpeed * Time.deltaTime;
    //    float y = eventData.delta.y * rotateSpeed * Time.deltaTime;

    //    transform.Rotate(0, -x, 0, Space.World);

    //    print("�巡��");
    //}
}
