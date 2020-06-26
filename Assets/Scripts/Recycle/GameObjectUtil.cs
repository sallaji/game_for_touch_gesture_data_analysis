using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//The utility is gonna be accesible throughout the game 
public class GameObjectUtil
{
    /*
   * 
   * since both of the methods above are static and it is required this 
   * class to be static the properties have to also be static as well. This way
   * they will always be stored inside of the reference of the class itself since
   * an instance of the game object utillity will never be created
   */

    //for looking up where each of the pools are located whenever a new instance is needed to be created
    private static Dictionary<RecycleGameObject, ObjectPool> pools
    = new Dictionary<RecycleGameObject, ObjectPool>();


    public static GameObject Instantiate(GameObject prefab, Vector3 pos)
    {
        GameObject instance = null;


        //makes surre the prefab being passed is a recycled game object
        var recycledScript = prefab.GetComponent<RecycleGameObject>();

        if (recycledScript != null)
        {
            var pool = GetObjectPool(recycledScript);
            instance = pool.NextObject(pos).gameObject;
        }
        else
        {
            instance = GameObject.Instantiate(prefab);
            instance.transform.position = pos;
        }
        return instance;
    }

    public static void Destroy(GameObject gameObject)
    {
        var recycleGameObject = gameObject.GetComponent<RecycleGameObject>();

        if (recycleGameObject != null)
        {
            recycleGameObject.ShutDown();
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }

    //returns the instance of the pool based on the game object being requested
    private static ObjectPool GetObjectPool(RecycleGameObject recycleGameObject)
    {
        ObjectPool objectPool = null;

        //if already contained 
        if (pools.ContainsKey(recycleGameObject))
        {
            objectPool = pools[recycleGameObject];
        }
        else
        {
            //a generic object that sits in the scene and is a way for storing the reference of the pool script on it
            var poolContainer = new GameObject(recycleGameObject.gameObject.name);

            objectPool = poolContainer.AddComponent<ObjectPool>();
            /*
             * set the pool prefab property to equal the reference being passed 
             * in so that the pool knows what type of objects to create
            */
            objectPool.prefab = recycleGameObject;
            pools.Add(recycleGameObject, objectPool);
        }

        return objectPool;
    }
}
