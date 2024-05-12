using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScrollAnimator : MonoBehaviour
{
    private float originalXPosition;
    private float originalYPosition;

    private Animator animator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public void Start()
    {
        originalXPosition = this.GetComponent<RectTransform>().anchoredPosition.x;
        originalYPosition = this.GetComponent<RectTransform>().anchoredPosition.y;
        animator = GetComponent<Animator>();
        OverrideAnimator();
    }

    private AnimationClip CreateExitAnimationClip()
    {
        AnimationClip clip = new AnimationClip();
        clip.name = "Scroll_Close";

        //X Key Frames
        Keyframe[] xFrames = new Keyframe[2];
        xFrames[0] = new Keyframe(0.0f, 1.0f);
        xFrames[1] = new Keyframe(1.0f, originalXPosition);
        clip.SetCurve("", typeof(RectTransform), "anchoredPosition.x", new AnimationCurve(xFrames));

        //Y Key frames 
        Keyframe[] yFrames = new Keyframe[2];
        xFrames[0] = new Keyframe(0.0f, 1.0f);
        xFrames[1] = new Keyframe(1.0f, originalYPosition);
        clip.SetCurve("", typeof(RectTransform), "anchoredPosition.y", new AnimationCurve(xFrames));

        return clip;
    }

    private void OverrideAnimator()
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in aoc.animationClips)
            {    
                if(a.name == "Scroll_Close")
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, CreateExitAnimationClip()));
                else
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a));
            }
            aoc.ApplyOverrides(anims);
            animator.runtimeAnimatorController = aoc;
    }
}
