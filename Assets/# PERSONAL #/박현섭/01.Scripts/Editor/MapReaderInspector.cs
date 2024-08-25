using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using 몰루;

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
        GUILayout.Label("OSM 지도 데이터 불러오는 버튼", style);

        //GUI.enabled = creator.transform.childCount == 0 ? true : false;

        if (GUILayout.Button("불러오기"))
        {
            mapReader.ReadMap();
        }
    }
}
