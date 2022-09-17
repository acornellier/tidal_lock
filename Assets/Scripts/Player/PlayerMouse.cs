using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMouse : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] float _interactRange = 2;

    [Inject] Player _player;

    Camera _mainCamera;

    readonly Collider2D[] _results = new Collider2D[16];

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void OnEnable()
    {
        _playerController.onLeftClick += HandleLeftClick;
        _playerController.onRightClick += HandleRightClick;
    }

    void OnDisable()
    {
        _playerController.onLeftClick -= HandleLeftClick;
        _playerController.onRightClick -= HandleRightClick;
    }

    void HandleLeftClick()
    {
        if (IsMouseOffScreen()) return;

        var mousePosition = MousePosition();

        var size = Physics2D.OverlapPointNonAlloc(mousePosition, _results);
        for (var i = 0; i < size; ++i)
        {
            var col = _results[i];
            var closestPoint = col.ClosestPoint(transform.position);
            if (Vector2.Distance(transform.position, closestPoint) > _interactRange)
                continue;

            var interactable = col.GetComponent<IInteractable>();
            interactable?.Interact(_player);
            return;
        }
    }

    void HandleRightClick()
    {
    }

    bool IsMouseOffScreen()
    {
        var position = _mainCamera.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        return position.x is < 0 or > 1 || position.y is < 0 or > 1;
    }

    Vector2 MousePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}