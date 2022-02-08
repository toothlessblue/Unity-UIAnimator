using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AnimationStep
{
    public RectTransform toAnimate;
    
    public Vector3 fromAnchoredPosition;
    public Vector3 fromDimensions;
    public Vector3 fromRelativePosition;

    public Vector3 toAnchoredPosition;
    public Vector3 toDimensions;
    public Vector3 toRelativePosition;

    public AnimationCurve curve;
    public float animationTime = .5f;
    public float withLastDelay = 0f;
    public When when = When.AfterLast;

    public delegate void onComplete();

    [HideInInspector] public UIAnimator animator;

    public AnimatableFields animatingFields;

    public AnimationStep next {
        get {
            if (this.animator.steps.Count - 1 == this.index) return null;
            return this.animator.steps[this.index + 1];
        }
    }
    public int index;

    public void animate(UIAnimator animator) {
        this.animator = animator;
        if (this.next != null) this.startNext(When.WithLast);
        this.animator.StartCoroutine(this._animate());
    }

    private IEnumerator _animate() {
        float step = 0f;
        while (step <= 1f) {
            step += Time.deltaTime / animationTime;

            this.toAnimate.anchoredPosition = Vector3.Lerp(this.fromAnchoredPosition, this.toAnchoredPosition, this.curve.Evaluate(step));

            yield return null;
        }

        if (this.next == null) {
            this.animator.onFinish();

        } else {
            this.startNext(When.AfterLast);
        }
    }

    private void startNext(When when) {
        if (this.next.when == When.WithLast && when == When.WithLast) {
            this.animator.StartCoroutine(this.delayStartNext());

        } else if (this.next.when == When.AfterLast && when == When.AfterLast) {
            this.next.animate(this.animator);
        }
    }

    private IEnumerator delayStartNext() {
        if (this.withLastDelay > 0) {
            yield return new WaitForSeconds(this.withLastDelay);
        }

        this.next.animate(this.animator);
    }

    public enum When {
        WithLast,
        AfterLast
    }
}


[Flags]
public enum AnimatableFields {
    Nothing = 0,
    X = 1 << 0,
    Y = 1 << 1,
    Z = 1 << 2,
    Width = 1 << 3,
    Height = 1 << 4,
    Top = 1 << 5,
    Bottom = 1 << 6,
    Left = 1 << 7,
    Right = 1 << 8,
}