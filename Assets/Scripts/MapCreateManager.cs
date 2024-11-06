using System.Collections.Generic;
using UnityEngine;

public class MapCreateManager : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private int maxPatternCnt;

    private Queue<GameObject> patternQueue = new Queue<GameObject>();
    private GameObject[] patterns;

    private Vector3 endPos = Vector3.zero;

    void Start()
    {
        patterns = Resources.LoadAll<GameObject>("Prefabs/Map Prefab");
        SelectMap();
        SelectMap();
        SelectMap();
        SelectMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SelectMap();
        }
    }

    public void CreateMap(GameObject pattern)
    {
        float degree = Random.value * 60;
        GameObject go = Instantiate(pattern);
        go.transform.position = endPos + new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad), 0) * distance * Random.value  - go.transform.Find("Start Point").localPosition;
        endPos = go.transform.Find("End Point").position;
        patternQueue.Enqueue(go);
        if (patternQueue.Count > maxPatternCnt) Destroy(patternQueue.Dequeue());
    }

    public void SelectMap()
    {
        CreateMap(patterns[Random.Range(0, patterns.Length)]); // ���� ���� ���þ˰����� ������ �߰�
    }
}
