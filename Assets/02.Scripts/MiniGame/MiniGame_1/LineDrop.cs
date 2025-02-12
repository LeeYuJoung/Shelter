using UnityEngine;
using UnityEngine.EventSystems;

public class LineDrop : Wire
    ,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(!(eventData.pointerDrag.GetComponent<Wire>().WireType == WireType))
        {
            return;
        }
        eventData.pointerDrag.GetComponent<Wire>().IsAnswer = true;
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
