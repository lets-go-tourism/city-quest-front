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
        meshRenderer.enabled = true;
        transform.position = TargetProp.PropObj.transform.TransformPoint(TargetProp.GetBoundsCenter());
        animator.enabled = true;
        enabled = true;
    }

    private float time = 0;
    private float waitTime = 0;

    private void OnEnable()
    {
        time = 0;
        waitTime = GetAnimTime();
    }

    public float GetAnimTime()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }

    private void Update()
    {
        time += waitTime;

        if (time > waitTime)
            enabled = false;

        transform.position = TargetProp.PropObj.transform.TransformPoint(TargetProp.GetBoundsCenter());
    }

    private void OnDisable()
    {
        TargetProp = null;
        meshRenderer.enabled = false;
        animator.enabled = false;
    }
}
