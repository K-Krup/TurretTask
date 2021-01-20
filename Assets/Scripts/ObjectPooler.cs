using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //defines type and quantity of object that will be pooled
    [System.Serializable]
    public class Pool
	{
        public string Tag;
        public GameObject Prefab;
        public int Size;
	}

    //allows to store multiple object pools
    [SerializeField]
    List<Pool> pools;

    //for easy access to any of the pool in case list will be expanded
    Dictionary<string, Queue<GameObject>> poolDictionary;

    //singleton instance
    static ObjectPooler instance;

    public static ObjectPooler GetInstance() { return instance; }
    

    void Awake()
    {
		if (instance == null)
		{
            instance = gameObject.GetComponent<ObjectPooler>();
		}
		else
		{
            Destroy(gameObject);
        }


        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
		{
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for(int i = 0; i< pool.Size; i++)
			{
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
			}

            poolDictionary.Add(pool.Tag, objectPool);
		}
    }

    public GameObject SpawObjectFromPool(string tag, Vector3 position, Quaternion rotation)
	{
		if (!poolDictionary.ContainsKey(tag))
		{
            Debug.LogWarning("Pool with tag " + tag + " does not exist");
            return null;
		}

        GameObject obj = poolDictionary[tag].Dequeue();
       
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        poolDictionary[tag].Enqueue(obj);


        return obj;
    }


}
