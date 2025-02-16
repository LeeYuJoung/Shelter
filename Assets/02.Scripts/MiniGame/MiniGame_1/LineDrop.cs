using UnityEngine;
using UnityEngine.EventSystems;

public class LineDrop : Wire
    ,IDropHandler
{
    [SerializeField] private RectTransform endPoint;

    public void OnDrop(PointerEventData eventData)
    {
        //Wire를 받아온다
        LineStretch wire = eventData.pointerDrag.GetComponent<LineStretch>();

        //null검사
        if(!Object.ReferenceEquals(wire, null))
        {
            //정답 아닐때
            if (!(wire.WireType == WireType))
            {
                return;
            }

            //정답일때
            wire.IsAnswer = true;
            wire.DefaultPosition = endPoint.position;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
