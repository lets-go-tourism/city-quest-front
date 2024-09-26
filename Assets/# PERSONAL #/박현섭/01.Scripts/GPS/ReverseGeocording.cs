using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGeocording : MonoBehaviour
{
    private List<Vector2> 정자동 = new List<Vector2>();
    private List<Vector2> 영화동 = new List<Vector2>();
    private List<Vector2> 고등동 = new List<Vector2>();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        for(int i = 1; i < 정자동.Count; i++)
        {
            Gizmos.DrawLine(정자동[i - 1], 정자동[i]);
        }
        Gizmos.DrawLine(정자동[정자동.Count - 1], 정자동[0]);

        // 위와같이 밑에도 정자동 영화동 기즈모를 그릴꺼야
        for(int i = 1; i < 영화동.Count; i++)
        {
            Gizmos.DrawLine(영화동[i - 1], 영화동[i]);
        }
        Gizmos.DrawLine(영화동[영화동.Count - 1], 영화동[0]);

        for(int i = 0; i < 고등동.Count; i++)
        {

        }
    }
#endif
}