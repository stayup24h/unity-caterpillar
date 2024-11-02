using System.Collections;
using UnityEngine;

public class BlinkGimmick : MonoBehaviour
{
    [SerializeField] private GameObject platform1;
    [SerializeField] private GameObject platform2;
    [SerializeField] private float targetTime; // On/Off 전환 시간

    private float time;
    private Coroutine runningCoroutine;


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= targetTime)
        {
            platform1.SetActive(!platform1.activeSelf);
            platform2.SetActive(!platform2.activeSelf);
            time = 0;
        }
    }

    IEnumerator Flicker(GameObject go) // 깜빡임 효과 수정 필요
    {
        int cnt = 0;

        while (cnt < 2)
        {
            go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.05f);
            go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
