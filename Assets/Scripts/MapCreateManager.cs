using System.Collections.Generic;
using UnityEngine;

public class MapCreateManager : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private GameObject patternPrefab;
    [SerializeField] private int maxPatternCnt;

    private Queue<GameObject> patternQueue = new Queue<GameObject>();

    private Vector3 endPos = Vector3.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateMap();
        }
    }

    public void CreateMap()
    {
        float degree = Random.value * 60;
        GameObject go = Instantiate(patternPrefab);
        go.transform.position = endPos + new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad), 0) * distance * Random.value;
        endPos = go.transform.Find("EndPos").position;
        patternQueue.Enqueue(go);
        if (patternQueue.Count > maxPatternCnt) Destroy(patternQueue.Dequeue());
    }
}
