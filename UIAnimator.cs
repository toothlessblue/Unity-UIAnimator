using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIAnimator : MonoBehaviour
{
    public delegate void OnFinish();

    public new string name;
    [HideInInspector] public List<bool> dropdowns;
    public List<AnimationStep> steps = new List<AnimationStep>();

    private OnFinish callback;

    public void animate() {
        this.callback = null;
        this.steps[0].animate(this);
    }

    public void animate(OnFinish callback) {
        this.callback = callback;
        this.steps[0].animate(this);
    }

    public void onFinish() {
        this.callback?.Invoke();
    }
}
