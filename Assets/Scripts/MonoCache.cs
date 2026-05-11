using System.Collections.Generic;
using UnityEngine;

public class MonoCache : MonoBehaviour
{
    public static List<MonoCache> allUpdate = new List<MonoCache>(10001);
    public static List<MonoCache> allFixedUpdate = new List<MonoCache>(10001);

    private void OnEnable() 
    {
        allUpdate.Add(this);
        AddFixedUpdate();
    }
    private void OnDisable()
    {
        allUpdate.Remove(this);
        RemoveFixedUpdate();
    }

    private void OnDestroy() 
    {
        allUpdate.Remove(this);
        RemoveFixedUpdate();
    } 

    protected void AddFixedUpdate() => allFixedUpdate.Add(this);

    protected void RemoveFixedUpdate() => allFixedUpdate.Remove(this);

    public void Tick() => OnTick();

    public void FixedTick() => OnFixedTick();

    public virtual void OnTick() { }

    public virtual void OnFixedTick() { }
}