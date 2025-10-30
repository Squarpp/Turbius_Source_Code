using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CablePoint : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform linePrefab;
    private RectTransform currentLine;
    private bool isDragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        currentLine = Instantiate(linePrefab, transform.parent);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || currentLine == null) return;

        Vector2 start = GetComponent<RectTransform>().anchoredPosition;
        Vector2 end;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out end
        );

        UpdateLine(currentLine, start, end);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        // Aquí puedes detectar si se soltó sobre otro CablePoint compatible.
    }

    void UpdateLine(RectTransform line, Vector2 start, Vector2 end)
    {
        Vector2 dir = end - start;
        line.anchoredPosition = start + dir / 2f;
        line.sizeDelta = new Vector2(dir.magnitude, 5); // Grosor = 5
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        line.rotation = Quaternion.Euler(0, 0, angle);
    }
}
