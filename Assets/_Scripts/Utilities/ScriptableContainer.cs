using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Container/ScriptableContainer")]
public class ScriptableContainer : ScriptableObject, IEnumerable<ScriptableObject>
{
    [SerializeField]
    private ScriptableObject[] scriptableObjects;

    public IEnumerable<T> GetByType<T>()
        where T : ScriptableObject => scriptableObjects.OfType<T>();

    public IEnumerable<T> GetByType<T>(Func<T, bool> predicate)
        where T : ScriptableObject => scriptableObjects.OfType<T>().Where(predicate);

    public IEnumerator<ScriptableObject> GetEnumerator()
    {
        return ((IEnumerable<ScriptableObject>)scriptableObjects).GetEnumerator();
    }

    public T GetFirstByType<T>()
        where T : ScriptableObject
    {
        if (typeof(T).GetInterfaces().Contains(typeof(IWeightable)))
        {
            var weightedItems = GetByType<T>()
                .OfType<IWeightable>()
                .Where(w => w.Weight > 0)
                .ToList();
            if (weightedItems.Count > 0)
            {
                double totalWeight = weightedItems.Sum(w => w.Weight);
                double randomWeight = UnityEngine.Random.Range(0f, (float)totalWeight);
                double cumulativeWeight = 0;
                foreach (var item in weightedItems)
                {
                    cumulativeWeight += item.Weight;
                    if (randomWeight <= cumulativeWeight)
                    {
                        return (T)item;
                    }
                }
            }
        }
        return GetByType<T>().FirstOrDefault();
    }

    public T GetFirstByType<T>(Func<T, bool> predicate)
        where T : ScriptableObject
    {
        if (typeof(T).GetInterfaces().Contains(typeof(IWeightable)))
        {
            var weightedItems = GetByType<T>(predicate)
                .OfType<IWeightable>()
                .Where(w => w.Weight > 0)
                .ToList();
            if (weightedItems.Count > 0)
            {
                double totalWeight = weightedItems.Sum(w => w.Weight);
                double randomWeight = UnityEngine.Random.Range(0f, (float)totalWeight);
                double cumulativeWeight = 0;
                foreach (var item in weightedItems)
                {
                    cumulativeWeight += item.Weight;
                    if (randomWeight <= cumulativeWeight)
                    {
                        return (T)item;
                    }
                }
            }
        }
        return GetByType<T>(predicate).FirstOrDefault();
    }

    
    public ScriptableObject this[int i] => scriptableObjects[i];
    IEnumerator IEnumerable.GetEnumerator() => scriptableObjects.GetEnumerator();  
}
