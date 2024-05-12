using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(menuName = "ScriptableObjects/NoteScriptable", fileName = "NoteScriptable")]
public class NoteScriptable : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Tempo tempo;
    [SerializeField] private NoteBehaviour noteBehaviour;
    [SerializeField] private NoteOrientation noteOrientation;

    public Sprite Sprite { get => sprite; private set => sprite = value; }
    public Tempo Tempo { get => tempo; private set => tempo = value; }
    public NoteBehaviour NoteBehaviour { get => noteBehaviour; private set => noteBehaviour = value; }
}

public enum NoteBehaviour
{
    Pause, Note
}

public enum NoteOrientation
{
    Up, Down, Center
}

public enum Tempo
{
    Whole = 1,
    Half = 2,
    Quarter = 4,
    Eighth = 8,
    Sixteenth = 16
}