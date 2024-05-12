using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollAnimator : UIEventTrigger
{
    private float originalXPosition;
    private float originalYPosition;
    private Animator animator;
    public Sprite openSprite;
    public Sprite closeSprite;


    public void Start()
    {
        originalXPosition = this.GetComponent<RectTransform>().anchoredPosition.x;
        originalYPosition = this.GetComponent<RectTransform>().anchoredPosition.y;
        animator = GetComponent<Animator>();
        fadeController = animator;
        OverrideAnimator();
    }

    private AnimationClip CreateExitAnimationClip(AnimationClip oldClip = null)
    {
        AnimationClip clip = oldClip ?? new AnimationClip();
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
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, CreateExitAnimationClip(a)));
                else
                    anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a));
            }
            aoc.ApplyOverrides(anims);
            animator.runtimeAnimatorController = aoc;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = openSprite;
        GetComponent<Animator>().Play("Scroll_Open");
        base.OnPointerClick(eventData);
    }
    public override void OnCancel(BaseEventData eventData)
    {
        GetComponent<Image>().sprite = closeSprite;
        GetComponent<Animator>().Play("Scroll_Close");
        base.OnCancel(eventData);
    }
}
