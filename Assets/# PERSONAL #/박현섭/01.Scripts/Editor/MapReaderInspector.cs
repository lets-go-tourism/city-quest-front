using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using ����;

[CustomEditor(typeof(MapReader))]
public class MapReaderInspector : Editor
{
    private MapReader mapReader;

    private void OnEnable()
    {
        mapReader = (MapReader)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        GUILayout.Label("OSM ���� ������ �ҷ����� ��ư", style);

        //GUI.enabled = creator.transform.childCount == 0 ? true : false;

        if (GUILayout.Button("�ҷ�����"))
        {
            mapReader.ReadMap();
        }
    }
}
