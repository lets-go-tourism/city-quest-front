using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCloud : MonoBehaviour
{
    public Prop TargetProp { get; private set; }
    private MeshRenderer meshRenderer;
    private Animator animator;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
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
        animator.enabled = true;
    }

    public float GetAnimTime()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    private void Update()
    {
        transform.position = TargetProp.PropObj.transform.TransformPoint(TargetProp.GetBoundsCenter());
    }

    private void OnDisable()
    {
        TargetProp = null;
        meshRenderer.enabled = false;
        animator.enabled = false;
    }
}
