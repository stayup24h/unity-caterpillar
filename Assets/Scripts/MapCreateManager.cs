using System.Collections.Generic;
using UnityEngine;

public class MapCreateManager : MonoBehaviour
{
    [SerializeField] private Transform target; // �ֹ��� �Ӹ� ��ġ
    [SerializeField] private float distance;
    [SerializeField] private int maxPatternCnt;

    private List<GameObject> patternQueue = new List<GameObject>();
    private GameObject[] patterns;

    private Vector3 endPos = Vector3.zero;

    void Start()
    {
        patterns = Resources.LoadAll<GameObject>("Prefabs/Map Prefab");
        for (int i = 0; i < maxPatternCnt; i++)
        {
            SelectMap();
        }
    }

    void Update()
    {
        if (target.position.x > patternQueue[2].transform.Find("Start Point").position.x)
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
        patternQueue.Add(go);
        if (patternQueue.Count > maxPatternCnt)
        {
            Destroy(patternQueue[0]);
            patternQueue.RemoveAt(0);
        }
    }

    public void SelectMap()
    {
        CreateMap(patterns[Random.Range(0, patterns.Length)]); // ���� ���� ���þ˰����� ������ �߰�
    }
}
