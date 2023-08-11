using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;

    float handleLimit = 1f;
    Vector2 inputVector = Vector2.zero;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }
    public Vector3 Direction { get { return new Vector3(Horizontal, Vertical, 0); } }

    RectTransform Container;

    Vector2 _joystickCenter = Vector2.zero;
    Vector3 _containerDefaultPosition;

    public UnityEvent OnTap;

    void Start()
    {
        handle.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        Container = GetComponent<RectTransform>();
        this._containerDefaultPosition = this.Container.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - _joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        handle.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        Container.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        _joystickCenter = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        Container.position = this._containerDefaultPosition;
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        this.OnTap.Invoke();
    }
}