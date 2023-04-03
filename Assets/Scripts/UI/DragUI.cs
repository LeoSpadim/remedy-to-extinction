using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IEndDragHandler
{
    public static bool isDragging;
    [SerializeField] private Canvas canvas;

    public void DragHandler(BaseEventData data)
    {
        if(/*!DragTarget.isDragging &&*/ !PauseMenu.PAUSED)
        {
            isDragging = true;
            PointerEventData pointerData = (PointerEventData) data;
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) canvas.transform, pointerData.position, canvas.worldCamera, out position);
            transform.position = canvas.transform.TransformPoint(position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}
