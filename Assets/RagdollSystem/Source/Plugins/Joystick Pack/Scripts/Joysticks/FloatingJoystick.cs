using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private Vector2 _startPos;
    private bool _isInitialized;
    
    protected override void Start()
    {
        _startPos = background.anchoredPosition;
        _isInitialized = true;
        
        base.Start();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!_isInitialized)
            return;
        
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!_isInitialized)
            return;
        
        base.OnPointerUp(eventData);
        background.anchoredPosition = _startPos;
    }
}