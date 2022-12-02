using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get 
        {
            if (instance == null)
            {
                //Create and cache instance
                GameObject go = new GameObject();
                instance = go.AddComponent<ObjectPool>();
                instance.Init();
            }
            return instance; 
        }
    }

    private class PoolData
    {
        private List<GameObject> objects;
        private Queue<GameObject> objectAvailable;
        private LinkedList<GameObject> objectsInUse;

        private GameObject _prefab;

        public void Initialize(GameObject prefab, int initialCount)
        {
            objects= new List<GameObject>();
            objectsInUse = new LinkedList<GameObject>();
            objectAvailable = new Queue<GameObject>();
            _prefab = prefab;

            for (int i = 0; i < initialCount; i++)
            {
                var obj = AddObject();
                objectAvailable.Enqueue(obj);
            }
        }

        public GameObject GetObject()
        {
            GameObject go;
            if(objectAvailable.Count > 0)
            {
                go = objectAvailable.Dequeue();
            }
            else
            {
                go = AddObject();
            }

            objectsInUse.AddFirst(go);
            go.SetActive(true);
            return go;
        }

        private GameObject AddObject()
        {
            var go = Instantiate(_prefab);
            go.transform.position = new Vector3(0, 100000, 0);
            objects.Add(go);
            return go;
        }
        
        public void FreeObject(GameObject obj)
        {
            if (objectsInUse.Contains(obj))
            {
                obj.transform.position = new Vector3(0, 100000, 0);
                obj.SetActive(false);
                objectAvailable.Enqueue(obj);
                objectsInUse.Remove(obj);
            }
        }

        public void CleanUp()
        {
            //Destroy all objects.
            for(int i = 0; i < objects.Count; i++)
            {
                Destroy(objects[i]);
            }
        }
    }

    private Dictionary<GameObject, PoolData> pool = new Dictionary<GameObject, PoolData>();
    private Dictionary<GameObject, PoolData> inUseToPool = new Dictionary<GameObject, PoolData>();

    private void Init()
    {
        //Not sure we need this.
    }

    public void CreatePool(GameObject prefab, int count = 10)
    {
        if (pool.ContainsKey(prefab))
        {
            return;
        }
        var data = new PoolData();
        data.Initialize(prefab, count);
        pool.Add(prefab, data);
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!pool.ContainsKey(prefab))
        {
            CreatePool(prefab);
        }

        var go = pool[prefab].GetObject();
        if (go != null)
        {
            inUseToPool.Add(go, pool[prefab]);
        }
        return go;
    }

    public void FreeObject(GameObject go)
    {
        if (inUseToPool.ContainsKey(go))
        {
            inUseToPool[go].FreeObject(go);
            inUseToPool.Remove(go);
        }
    }
}
