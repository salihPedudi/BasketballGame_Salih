using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{

    [System.Serializable]
    public struct Pool
    {
        public Queue<GameObject> pooledObjects;
        public GameObject objectPrefab;
        public int poolSize;
        public Transform parent;
    }

    [SerializeField] private Pool[] pools = null;

    public static ObjectPooling Instance;
    public bool isPooled = false;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }


    void Start()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].pooledObjects = new Queue<GameObject>();

            for (int j = 0; j < pools[i].poolSize; j++)
            {
                GameObject _obj = Instantiate(pools[i].objectPrefab);
                _obj.transform.SetParent(pools[i].parent);
                _obj.SetActive(false);

                pools[i].pooledObjects.Enqueue(_obj);
            }
        }
        isPooled = true;
    }

    public GameObject GetPooledObject(int _objType)
    {

        if (_objType >= pools.Length)
            return null;

        GameObject _obj = pools[_objType].pooledObjects.Dequeue();

        _obj.SetActive(true);

        pools[_objType].pooledObjects.Enqueue(_obj);

        return _obj;
    }
}
