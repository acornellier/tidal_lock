using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Action onLeftClick;
    public Action onRightClick;
    public Action onBag;

    InputActions.PlayerActions _actions;

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

    void HandleLeftClick(InputAction.CallbackContext obj)
    {
        onLeftClick?.Invoke();
    }

    void HandleRightClick(InputAction.CallbackContext obj)
    {
        onRightClick?.Invoke();
    }

    void HandleBag(InputAction.CallbackContext obj)
    {
        onBag?.Invoke();
    }
}