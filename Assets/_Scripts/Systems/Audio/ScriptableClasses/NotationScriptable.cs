using UnityEngine;
using Kingdom.Enums;


namespace Kingdom.Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Notation", fileName = "Notation")]
    public class NotationScriptable : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private Tempo tempo;
        [SerializeField] private NotationBehaviour noteBehaviour;
        [SerializeField] private NotationOrientation noteOrientation;

        public Sprite Sprite { get => sprite; private set => sprite = value; }
        public Tempo Tempo { get => tempo; private set => tempo = value; }
        public NotationBehaviour NoteBehaviour { get => noteBehaviour; private set => noteBehaviour = value; }
        public NotationOrientation NoteOrientation{ get => noteOrientation;}
    }
}

