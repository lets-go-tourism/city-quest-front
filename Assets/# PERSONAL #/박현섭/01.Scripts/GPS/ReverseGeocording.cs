using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGeocording : MonoBehaviour
{
    private List<Vector2> ���ڵ� = new List<Vector2>();
    private List<Vector2> ��ȭ�� = new List<Vector2>();
    private List<Vector2> �� = new List<Vector2>();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        for(int i = 1; i < ���ڵ�.Count; i++)
        {
            Gizmos.DrawLine(���ڵ�[i - 1], ���ڵ�[i]);
        }
        Gizmos.DrawLine(���ڵ�[���ڵ�.Count - 1], ���ڵ�[0]);

        // ���Ͱ��� �ؿ��� ���ڵ� ��ȭ�� ����� �׸�����
        for(int i = 1; i < ��ȭ��.Count; i++)
        {
            Gizmos.DrawLine(��ȭ��[i - 1], ��ȭ��[i]);
        }
        Gizmos.DrawLine(��ȭ��[��ȭ��.Count - 1], ��ȭ��[0]);

        for(int i = 0; i < ��.Count; i++)
        {

        }
    }
#endif
}