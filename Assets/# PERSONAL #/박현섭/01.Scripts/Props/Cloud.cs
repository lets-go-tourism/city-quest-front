using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cloud : MonoBehaviour
{
    public Prop TargetProp { get; private set; }
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        enabled = false;
    }

    public void StartSetting(Prop target)
    {
        this.TargetProp = target;
        enabled = true;
    }

    private void OnEnable()
    {
        meshRenderer.enabled = true;
        transform.position = TargetProp.PropObj.transform.TransformPoint(TargetProp.GetBoundsCenter());
    }

    private void Update()
    {
        transform.position = TargetProp.PropObj.transform.TransformPoint(TargetProp.GetBoundsCenter());
        transform.localScale = Vector3.one * 130 * Camera.main.transform.position.y / 800;
    }

    private void OnDisable()
    {
        TargetProp = null;
        meshRenderer.enabled = false;
    }
}
