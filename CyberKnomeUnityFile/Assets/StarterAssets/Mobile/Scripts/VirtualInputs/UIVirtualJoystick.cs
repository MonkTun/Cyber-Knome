using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIVirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }
    
    [Header("Rect References")]
    public RectTransform thisRect;
    public RectTransform containerRect;
    public RectTransform handleRect;

    [Header("Settings")]
    public float joystickRange = 50f;
    public float magnitudeMultiplier = 1f;
    public bool invertXOutputValue;
    public bool invertYOutputValue;

    [Header("Output")]
    public Event joystickOutputEvent;

    [Header("Graphics")]
    public bool useHideOption;
    public Image[] joystickVisuals;

	[Header("Settings")]
    public bool isLeftJoystick;
    
    //for dynamic movement
    bool isDragging;
    Vector2 originalPos;

    void Start()
    {
        SetupHandle();
        originalPos = containerRect.anchoredPosition;
    }

    private void SetupHandle()
    {
        if(handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero);
        }

        if (useHideOption)
        {
            foreach (Image img in joystickVisuals)
            {
                img.color = Color.clear;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);

        if (useHideOption)
        {
            foreach (Image img in joystickVisuals)
            {
                img.color = Color.white;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging && isLeftJoystick)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, eventData.position, eventData.pressEventCamera, out Vector2 localCursor))
            {
                isDragging = true;
                containerRect.anchoredPosition = localCursor;
            }
        } 
        else
	    {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, eventData.position, eventData.pressEventCamera, out Vector2 localCursor))
            {
                isDragging = true;
                containerRect.anchoredPosition = Vector2.Lerp(containerRect.anchoredPosition, localCursor, Time.deltaTime * (isLeftJoystick ? Settings.LJSensitivity : Settings.RJSensitivity));
            }
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);
        
        position = ApplySizeDelta(position);
        
        Vector2 clampedPosition = ClampValuesToMagnitude(position);

        Vector2 outputPosition = ApplyInversionFilter(position);

        OutputPointerEventValue(outputPosition * magnitudeMultiplier);

        if (handleRect)
        {
            UpdateHandleRectPosition(clampedPosition * joystickRange);
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        OutputPointerEventValue(Vector2.zero);

        if(handleRect)
        {
             UpdateHandleRectPosition(Vector2.zero);
        }

        if (useHideOption)
        {
            foreach (Image img in joystickVisuals)
            {
                img.color = Color.clear;
            }
        }

        containerRect.anchoredPosition = originalPos;
    }

    private void OutputPointerEventValue(Vector2 pointerPosition)
    {
        joystickOutputEvent.Invoke(pointerPosition);
    }

    private void UpdateHandleRectPosition(Vector2 newPosition)
    {
        handleRect.anchoredPosition = newPosition;
    }

    Vector2 ApplySizeDelta(Vector2 position)
    {
        float x = (position.x/containerRect.sizeDelta.x) * 2.5f;
        float y = (position.y/containerRect.sizeDelta.y) * 2.5f;
        return new Vector2(x, y);
    }

    Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        return Vector2.ClampMagnitude(position, 1);
    }

    Vector2 ApplyInversionFilter(Vector2 position)
    {
        if(invertXOutputValue)
        {
            position.x = InvertValue(position.x);
        }

        if(invertYOutputValue)
        {
            position.y = InvertValue(position.y);
        }

        return position;
    }

    float InvertValue(float value)
    {
        return -value;
    }
}