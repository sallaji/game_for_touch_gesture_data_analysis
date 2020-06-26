using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    public RecycleGameObject prefab;

    private List<RecycleGameObject> poolInstances = new List<RecycleGameObject>();

    public List<RecycleGameObject> PoolInstances
    {
        get
        {
            return poolInstances;
        }
        set
        {
            poolInstances = value;
        }
    }

    //creates an instance when needed
    private RecycleGameObject CreateInstance(Vector3 position)
    {
        //a clone that the object pool manages. This is the only place in the game where this is directly instantiated
        var clone = GameObject.Instantiate(prefab);
        clone.transform.position = position;
        
        poolInstances.Add(clone);

        return clone;
    }

    //returns the instance when requested
    public RecycleGameObject NextObject(Vector3 position)
    {
        //Container for storing the recycle game object to be created
        RecycleGameObject instance = null;

        foreach(var gameObject in poolInstances)
        {
            if(!gameObject.gameObject.activeSelf)
            {
                instance = gameObject;
                instance.transform.position = position;
            }
        }

        if(instance == null)
        {
            //Creates a new instance everytime its requested from the object pool
            instance = CreateInstance(position);
        }

        //since this is a RecycleGameObject whenever a new instance of it is created the Restart method will be called
        instance.Restart();

        return instance;
    }
}
