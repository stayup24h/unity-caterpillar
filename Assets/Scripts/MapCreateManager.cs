using System.Collections.Generic;
using UnityEngine;

public class MapCreateManager : MonoBehaviour
{
    [SerializeField] private Transform mapPool;
    [SerializeField] private Transform target; // �ֹ��� �Ӹ� ��ġ
    [SerializeField] private float distance;
    [SerializeField] private int maxPatternCnt;

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
        if (target.position.x > mapPool.GetChild(2).Find("Start Point").position.x)
        {
            SelectMap();
        }
    }

    public void CreateMap(GameObject pattern)
    {
        float degree = Random.value * 60;
        GameObject go = Instantiate(pattern, mapPool);
        go.transform.position = endPos + new Vector3(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad), 0) * distance * Random.value  - go.transform.Find("Start Point").localPosition;
        endPos = go.transform.Find("End Point").position;
        if (mapPool.childCount > maxPatternCnt)
        {
            Destroy(mapPool.GetChild(0).gameObject);
        }
    }

    public void SelectMap()
    {
        CreateMap(patterns[Random.Range(0, patterns.Length)]); // ���� ���� ���þ˰����� ������ �߰�
    }
}
