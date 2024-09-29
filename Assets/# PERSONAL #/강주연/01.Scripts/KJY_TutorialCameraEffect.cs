using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KJY_TutorialCameraEffect : MonoBehaviour
{

    [SerializeField] private Transform target_pos;
    private Camera cam;
    public float rotation_speed = 10;
    public float move_speed = 5;
    public float camera_distance = 100;

    public float targetPov = 30f;
    public float resetPov = 60f;

    public Transform firstTr;
    private void Start()
    {
        cam = Camera.main;
    }

    public void Test()
    {
        firstTr = cam.transform;
        Debug.Log(firstTr.transform.position);
        Debug.Log(firstTr.transform.rotation);
        Vector3 target = target_pos.transform.position - cam.transform.position;
        StartCoroutine(RotateVectorCoroutine(target, cam.transform, true));
        StartCoroutine(AdjustCameraFOV(targetPov, 2f));
    }

    //부드럽게 회전시켜주는 함수
    IEnumerator RotateVectorCoroutine(Vector3 target, Transform tr, bool isPosition)
    {
        while (true)
        {
            if (isPosition == true)
            {
                tr.transform.rotation = Quaternion.Lerp(tr.transform.rotation, Quaternion.LookRotation(target), Time.deltaTime * rotation_speed);
            }
            else
            {
                tr.transform.rotation = Quaternion.Lerp(tr.transform.rotation, Quaternion.Euler(target), Time.deltaTime * rotation_speed);
            }

            if (AreQuaternionsSimilar(tr.transform.rotation, Quaternion.LookRotation(target)))
            {
                Debug.Log("stop_rotation");
                yield break; 
            }

            yield return null;
        }
    }

    bool AreQuaternionsSimilar(Quaternion a, Quaternion b, float tolerance = 0.001f)
    {
        float angleDiff = Quaternion.Angle(a, b);

        return Mathf.Abs(angleDiff) < tolerance;
    }

    IEnumerator MoveCamera(Vector3 target)
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, target, Time.deltaTime * move_speed);
            if (Vector3.Distance(cam.transform.position, target) < camera_distance)
            {
                Debug.Log("stop_dis");
                break;
            }
            yield return null;
        }
    }

    IEnumerator AdjustCameraFOV(float targetFOV, float duration)
    {
        float startFOV = cam.fieldOfView; 
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, timeElapsed / duration);
            timeElapsed += Time.deltaTime;

            yield return null; 
        }

        cam.fieldOfView = targetFOV;
    }

    public void Reset()
    {
        StartCoroutine(RotateVectorCoroutine(new Vector3(90, 0, 0), cam.transform, false));
        StartCoroutine(AdjustCameraFOV(resetPov, 2));
    }
}
