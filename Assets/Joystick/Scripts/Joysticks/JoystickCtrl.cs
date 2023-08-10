using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickCtrl : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    /* component refs */
    public RectTransform background;
    public RectTransform handle;

    /* public vars */
    public float handleLimit = 1f;
    protected Vector2 inputVector = Vector2.zero;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }
    public Vector3 Direction { get { return new Vector3(Horizontal, Vertical, 0); } }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }
}