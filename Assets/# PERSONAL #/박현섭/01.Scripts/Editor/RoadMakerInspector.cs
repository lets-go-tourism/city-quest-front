using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(RoadMaker))]
public class RoadMakerInspector : Editor
{
    private RoadMaker roadMaker;

    private void OnEnable()
    {
        roadMaker = (RoadMaker)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        GUILayout.Label("���� �����͸� �������� ���θ� ����", style);

        //GUI.enabled = creator.transform.childCount == 0 ? true : false;

        if (GUILayout.Button("�����ϱ�"))
        {
            roadMaker.MakeRoad();
        }
    }
}
