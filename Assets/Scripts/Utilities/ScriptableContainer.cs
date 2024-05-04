using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Container/ScriptableContainer")]
public class ScriptableContainer : ScriptableObject
{

    [SerializeField]
    private ScriptableObject[] scriptableObjects;

    public IEnumerable<T> GetByType<T>() where T : ScriptableObject => scriptableObjects.OfType<T>();
    
}