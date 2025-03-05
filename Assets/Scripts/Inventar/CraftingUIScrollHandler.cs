using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CraftingUIScrollHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
{
    private bool isMouseOver = false;
    public float ScrollSpeed = 20;

    public Transform Panel;

    private float ScrollAmount = 0;
    private float OriginalY;

    public float ChildSize = 50f;

    private RectTransform rect;

    private void Start()
    {
        OriginalY = transform.position.y;

        Canvas.ForceUpdateCanvases();
        rect = Panel.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    public void OnScroll(PointerEventData eventData)
    {
        if (isMouseOver)
        {
            if (Input.mouseScrollDelta.y > 0) { ScrollAmount += 1 * ScrollSpeed;  }
            if (Input.mouseScrollDelta.y < 0) { ScrollAmount -= 1 * ScrollSpeed;  }
        }

        if (ScrollAmount > 0)
        {
            ScrollAmount = 0;
        }

        if (ScrollAmount < -(ChildSize * Panel.childCount))
        {
            ScrollAmount = -(ChildSize * Panel.childCount);
        }

        Panel.transform.position = new Vector2(Panel.transform.position.x, OriginalY - ScrollAmount);
        Debug.Log(ScrollAmount);
    }
}
