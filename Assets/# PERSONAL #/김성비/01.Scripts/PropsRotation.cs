using UnityEngine;

public class PropsRotation : MonoBehaviour
{
    public float rotateSpeed = 10.0f;

    void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }

    //public void OnDrag(PointerEventData eventData)
    //{
    //    float x = eventData.delta.x * rotateSpeed * Time.deltaTime;
    //    float y = eventData.delta.y * rotateSpeed * Time.deltaTime;

    //    transform.Rotate(0, -x, 0, Space.World);

    //    print("µå·¡±×");
    //}
}
