using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Action onLeftClick;
    public Action onRightClick;
    public Action onBag;

    InputActions.PlayerActions _actions;
    bool _pointerOverUi;

    void Awake()
    {
        _actions = new InputActions().Player;
        _actions.LeftClick.performed += HandleLeftClick;
        _actions.RightClick.performed += HandleRightClick;
        _actions.Bag.performed += HandleBag;
    }

    void OnEnable()
    {
        EnableControls();
    }

    void OnDisable()
    {
        DisableControls();
    }

    void Update()
    {
        _pointerOverUi = EventSystem.current.IsPointerOverGameObject();
    }

    public Vector2 MoveValue()
    {
        return _actions.Move.ReadValue<Vector2>();
    }

    void EnableControls()
    {
        _actions.Enable();
    }

    void DisableControls()
    {
        _actions.Disable();
    }

    void HandleLeftClick(InputAction.CallbackContext ctx)
    {
        if (_pointerOverUi) return;

        onLeftClick?.Invoke();
    }

    void HandleRightClick(InputAction.CallbackContext ctx)
    {
        if (_pointerOverUi) return;

        onRightClick?.Invoke();
    }

    void HandleBag(InputAction.CallbackContext ctx)
    {
        onBag?.Invoke();
    }
}