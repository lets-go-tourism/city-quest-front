using UnityEngine;

public class PropsRotation : MonoBehaviour
{
    public float rotateSpeed = 10.0f;

    // 제자리 회전
    void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }

    // 드래그로 회전
    //public void OnDrag(PointerEventData eventData)
    //{
    //    float x = eventData.delta.x * rotateSpeed * Time.deltaTime;
    //    float y = eventData.delta.y * rotateSpeed * Time.deltaTime;

    //    transform.Rotate(0, -x, 0, Space.World);

    //    print("드래그");
    //}
}
