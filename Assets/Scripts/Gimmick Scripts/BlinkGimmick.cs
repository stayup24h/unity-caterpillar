using System.Collections;
using UnityEngine;

public class BlinkGimmick : MonoBehaviour
{
    [SerializeField] private float targetTime; // On/Off 전환 시간

    private GameObject[] platforms;
    private bool[] isFlicking;

    private float time;

    void Start()
    {
        platforms = new GameObject[transform.childCount];
        isFlicking = new bool[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            platforms[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (time > targetTime - 1)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                if (platforms[i].activeSelf && !isFlicking[i])
                {
                    isFlicking[i] = true;
                    StartCoroutine(Flicker(i));
                }
            }
        }
        if (time >= targetTime)
        {
            foreach (GameObject platform in platforms)
            {
                platform.GetComponent<BlinkPlatform>().PlatformDisappear();
                platform.SetActive(!platform.activeSelf);
            }
            time = 0;
        }
    }

    IEnumerator Flicker(int idx) // 깜빡임 효과 수정 필요
    {
        GameObject platform = platforms[idx];
        int cnt = 0;

        while (cnt < 2)
        {
            platform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.05f);
            platform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.45f);

            cnt++;
        }

        isFlicking[idx] = false;
    }
}
