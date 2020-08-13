using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;

    private const int SIZE = 6;
    private float[] pos = new float[SIZE];  //각 이미지의 위치
    private float distance;                 //pos 사이의 간격
    private float targetPos, currentPos;
    private int targetIndex;
    private bool isDrag;

    void Start()
    {
        // 거리에 따라 0 ~ 1인 pos 대입
        distance = 1f / (SIZE - 1);

        for (int i = 0; i < SIZE; i++)
            pos[i] = distance * i;
    }

    float SetPos()
    {
        // 절반거리를 기준으로 가까운 위치를 반환
        for (int i = 0; i < SIZE; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        }
        return 0;   //실행하지 않는 부분
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentPos = SetPos();
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        // 절반거리를 넘지 않아도 마우스를 빠르게 이동하면
        if (currentPos == targetPos)
        {
            // 스크롤이 왼쪽으로 빠르게 이동시 목표가 하나 감소
            if(eventData.delta.x > 18 && currentPos - distance >= 0)
            {
                --targetIndex;
                targetPos = currentPos - distance;
            }
            // 스크롤이 오른쪽으로 빠르게 이동시 목표가 하나 증가
            else if (eventData.delta.x < -18 && currentPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = currentPos + distance;
            }
        }
    }

    void Update()
    {
        if (!isDrag)    // 드래그 중일때만 자연스럽게 이동
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.3f);
    }
}
