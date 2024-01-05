using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Cinemachine;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform background;
    public RectTransform handle;

    float handleLimit = 1f;
    Vector2 inputVector = Vector2.zero;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }

    RectTransform Container;
    CinemachineOrbitalTransposer Transposer;
    Vector2 _joystickCenter = Vector2.zero;
    Vector3 _containerDefaultPosition;

    public UnityEvent OnTap;

    void Start()
    {
        //handle.gameObject.SetActive(false);
        //background.gameObject.SetActive(false);
        Container = GetComponent<RectTransform>();
        Transposer = GameObject.Find("CM vcam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
        this._containerDefaultPosition = this.Container.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Transposer.m_XAxis.m_InputAxisName = "";
        Vector2 direction = eventData.position - _joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Transposer.m_XAxis.m_InputAxisName = "";
        //handle.gameObject.SetActive(true);
        //background.gameObject.SetActive(true);
        Container.position = eventData.position;
        handle.anchoredPosition = Vector2.zero;
        _joystickCenter = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Transposer.m_XAxis.m_InputAxisName = "Mouse X";
        //handle.gameObject.SetActive(false);
        //background.gameObject.SetActive(false);
        Container.position = this._containerDefaultPosition;
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
        this.OnTap.Invoke();
    }
}