using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerController : MonoBehaviour
{
    public InputActions.PlayerActions actions;

    public Action onLeftClick;
    public Action onRightClick;

    void Awake()
    {
        actions = new InputActions().Player;
        actions.LeftClick.performed += HandleLeftClick;
        actions.RightClick.performed += HandleRightClick;
    }

    void OnEnable()
    {
        EnableControls();
    }

    void OnDisable()
    {
        DisableControls();
    }

    void EnableControls()
    {
        actions.Enable();
    }

    void DisableControls()
    {
        actions.Disable();
    }

    void HandleLeftClick(InputAction.CallbackContext obj)
    {
        onLeftClick?.Invoke();
    }

    void HandleRightClick(InputAction.CallbackContext obj)
    {
        onRightClick?.Invoke();
    }
}