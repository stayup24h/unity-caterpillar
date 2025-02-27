using UnityEngine;

public class MapCreateManager : MonoBehaviour
{
    [SerializeField] private Transform mapPool;
    [SerializeField] private GameObject flag;
    [SerializeField] private Transform target; // head
    [SerializeField] private float distance;
    [SerializeField] private int maxPatternCnt;

    private Vector3 endPos = Vector3.zero;

    void Start()
    {
        if (GameManager.Instance.MapType == MapType.stage)
        {
            GameObject stage = GameManager.Instance.mapPatterns.Find(e => e.name == "Stage " + GameManager.Instance.StageNum);
            if (stage == null)
            {
                Debug.Log("스테이지 번호 오류");
                InitMap(GameManager.Instance.mapPatterns[0]); // Test
            }
            else
            {
                InitMap(stage);
            }
            Instantiate(flag, endPos, Quaternion.identity);
        }
        else if (GameManager.Instance.MapType == MapType.remix)
        {
            InitMap(GetRandomMap());
            for (int i = 1; i < maxPatternCnt; i++)
            {
                CreateMap(GetRandomMap());
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.MapType == MapType.remix)
        {
            if (target.position.x > mapPool.GetChild(2).Find("Start Point").position.x)
            {
                CreateMap(GetRandomMap());
            }
        }
    }

    private void InitMap(GameObject pattern)
    {
        GameObject go = Instantiate(pattern, mapPool);
        go.transform.position = Vector3.down - go.transform.Find("Start Point").localPosition;
        endPos = go.transform.Find("End Point").position;
    }

    private void CreateMap(GameObject pattern)
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

    private GameObject GetRandomMap()
    {
        return GameManager.Instance.mapPatterns[Random.Range(0, GameManager.Instance.mapPatterns.Count)]; // ���� ���� ���þ˰����� ������ �߰�
    }
}
