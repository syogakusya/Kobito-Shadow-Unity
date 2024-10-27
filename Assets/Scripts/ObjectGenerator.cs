using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab;
    private Queue<GameObject> objects;
    private List<GameObject> objectsList;
    public float repeatTime = 0.1f;
    public int objectMax = 100;
    public float generatRange = 20;

    void Awake()
    {
        objects = new Queue<GameObject>();
        objectsList = new List<GameObject>();
        for (int i = 0; i < objectMax; i++)
        {
            GameObject o = Instantiate(objectPrefab, new Vector3(Random.Range(transform.position.x - generatRange / 2.0f, transform.position.x + generatRange / 2.0f), transform.position.y, transform.position.z), Quaternion.identity);
            objects.Enqueue(o);
            objectsList.Add(o);
            o.transform.SetParent(transform);
        }
        InvokeRepeating("UpdatePrefab", 0.0f, repeatTime);
    }

    void Update()
    {
        if (objectsList.Count > objectMax) {
            for (int i = 0; i < objectsList.Count - objectMax; i++)
            {
                GameObject o = objectsList[objectsList.Count -1];
                objectsList.Remove(o);
                Destroy(o);
            }
        }

        if (objectsList.Count < objectMax)
        {
            for (int i = 0;i < objectMax - objectsList.Count; i++)
            {
                GameObject o = Instantiate(objectPrefab, new Vector3(Random.Range(transform.position.x - generatRange / 2.0f, transform.position.x + generatRange / 2.0f), transform.position.y, transform.position.z), Quaternion.identity);
                objects.Enqueue(o);
                objectsList.Add(o);
                o.transform.SetParent(transform);
            }
        }

        foreach(GameObject o in objectsList)
        {
            if(o.transform.position.y < -30)
            {
                ColletObject(o);
            }
        }
    }

    private void UpdatePrefab()
    {
        if (objects.Count <= 0)
        {
            return;
        }

        GameObject o = objects.Dequeue();
        o.SetActive(true);
    }

    public void ColletObject(GameObject co)
    {
        co.SetActive(false);
        co.transform.position = new Vector3(Random.Range(transform.position.x - generatRange / 2.0f, transform.position.x + generatRange / 2.0f), transform.position.y, transform.position.z);
        var rigidbody2D = co.GetComponent<Rigidbody2D>();
        var rigidbody3D = co.GetComponent<Rigidbody>();
        if(rigidbody2D == null)
        {
            rigidbody3D.velocity = Vector3.zero;
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
        }
        objects.Enqueue(co);
    }
}
