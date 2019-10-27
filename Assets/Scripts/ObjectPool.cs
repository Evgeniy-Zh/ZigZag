using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject SampleObject;
    public int size = 20;
    private Queue<GameObject> queue = new Queue<GameObject>();
    void Start()
    {
        if (queue.Count == 0) Init();
    }

    private void Init()
    {
        if (SampleObject != null)
            for (int i = 0; i < size; i++)
            {
                var obj = Instantiate(SampleObject);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }

    }

    public void DisableAll()
    {
        foreach(var ob in queue)
        {
            ob.SetActive(false);
        }
    }

    public GameObject SpawnObject(Vector3 pos)
    {
        if (queue.Count == 0) Init();
        var obj = queue.Dequeue();
        obj.SetActive(true);
        obj.transform.position = pos;
        queue.Enqueue(obj);
        IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
        pooledObj?.OnObjectSpawned();
        return obj;
    }
}
