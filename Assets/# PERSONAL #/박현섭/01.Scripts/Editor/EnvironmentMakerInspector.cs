using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(EnvironmentMaker))]
public class EnvironmentMakerInspector : Editor
{
    private EnvironmentMaker environmentMaker;

    private void OnEnable()
    {
        environmentMaker = (EnvironmentMaker)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.yellow;
        GUILayout.Label("���� �����͸� �������� ȯ���� ����", style);

        //GUI.enabled = creator.transform.childCount == 0 ? true : false;

        if (GUILayout.Button("�����ϱ�"))
        {
            environmentMaker.MakeEnvironment();
        }
    }
}
