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
        GUILayout.Label("지도 데이터를 바탕으로 도로를 생성", style);

        //GUI.enabled = creator.transform.childCount == 0 ? true : false;

        if (GUILayout.Button("생성하기"))
        {
            roadMaker.MakeRoad();
        }
    }
}
