using System.Collections;
using System.Collections.Generic;
using Kingdom.Effects;
using UnityEngine;

public class EffectApplied : MonoBehaviour
{
    public EffectDTO effectDTO;

    void Start() { }

    void Update() { }

    public void SetData(EffectDTO effectDTO)
    {
        this.effectDTO = effectDTO;
        //Set Text
        //Handle Hover with text and circle/symbol on the target game object
    }
}
