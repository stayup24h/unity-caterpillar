using UnityEngine;
using UnityEngine.EventSystems;

public class InfiniteScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform[] scrollElements;
    [SerializeField] private float swipeThreshold = 50f; // 스와이프 거리
    [SerializeField] private float snapSpeed = 10f; // 스크롤 속도

    private int currentIndex = 0;
    private float canvasSize;
    private Vector2 dragStartPos;
    private bool isLerping = false;
    private float lerpTime = 0;

    void Start()
    {
        canvasSize = canvas.GetComponent<RectTransform>().sizeDelta.y;
        UpdateElementPositions();
    }

    void Update()
    {
        if (isLerping)
        {
            lerpTime += Time.deltaTime * snapSpeed;

            if (lerpTime >= 1)
            {
                lerpTime = 1;
                isLerping = false;
            }

            for (int i = 0; i < scrollElements.Length; i++)
            {
                scrollElements[i].position = Vector2.Lerp(
                    scrollElements[i].position,
                    GetOffset(i),
                    lerpTime
                );
            }

        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos = eventData.position;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        float dragDelta = eventData.position.y - dragStartPos.y;
        if (Mathf.Abs(dragDelta / canvasSize) >= 1)
        {
            if (dragDelta > 0)
            {
                currentIndex = (currentIndex + 1) % scrollElements.Length;
                dragDelta -= canvasSize;
            }
            else
            {
                currentIndex = (currentIndex - 1) % scrollElements.Length;
                dragDelta += canvasSize;
            }
            dragStartPos = eventData.position;
        }

        for (int i = 0; i < scrollElements.Length; i++)
        {
            scrollElements[i].position = GetOffset(i) + new Vector2(0, dragDelta);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        float dragDistance = eventData.position.y - dragStartPos.y;
        float delta = dragDistance;
        if (Mathf.Abs(dragDistance) > swipeThreshold)
        {
            if (dragDistance > 0)
            {
                currentIndex = (currentIndex + 1) % scrollElements.Length;
                delta -= canvasSize;
            }
            else
            {
                currentIndex = (currentIndex - 1 + scrollElements.Length) % scrollElements.Length;
                delta += canvasSize;
            }
        }
        UpdateElementPositions(delta);

        lerpTime = 0;
        isLerping = true;
    }

    private void UpdateElementPositions(float delta = 0)
    {
        for (int i = 0; i < scrollElements.Length; i++)
        {
            scrollElements[i].position = GetOffset(i) + new Vector2(0, delta);
        }
    }

    private Vector2 GetOffset(int index)
    {
        return new Vector2(0, -(((index - currentIndex + scrollElements.Length + 1) % scrollElements.Length) - 1) * canvasSize);
    }
}
