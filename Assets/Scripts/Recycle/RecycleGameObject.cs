using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IRecycle
{
    void Restart();
    void ShutDown();
}

//tells how to shut down and restart a gameObject
public class RecycleGameObject : MonoBehaviour
{

    private List<IRecycle> recycleComponents;

    private void Awake()
    {
        var components = GetComponents<MonoBehaviour>();
        recycleComponents = new List<IRecycle>();

        foreach(var component in components)
        {
            if(component is IRecycle)
            {
                recycleComponents.Add(component as IRecycle);
            }
        }
    }

    //call it when attempting to reinstantiate a game object that already exists
    public void Restart()
    {
        //allows the gameobject to be activaded/deactivated but it still exists in the hierarchy of the gamescene
        gameObject.SetActive(true);

        foreach (var component in recycleComponents)
        {
            component.Restart();
        }
    }

    public void ShutDown()
    {
        gameObject.SetActive(false);
        foreach(var component in recycleComponents)
        {
            component.ShutDown();
        }
    }

}
