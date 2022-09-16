using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouse : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] SpriteRenderer _stackMarker;

    [SerializeField] float _interactRange = 2;

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

    void Update()
    {
        _stackMarker.transform.position = RoundedMousePosition();
    }

    void HandleLeftClick()
    {
        if (IsMouseOffScreen()) return;

        var mousePosition = RoundedMousePosition();

        var size = Physics2D.OverlapPointNonAlloc(mousePosition, _results);
        for (var i = 0; i < size; ++i)
        {
            var interactable = _results[i].GetComponent<Interactable>();
            if (!interactable) continue;

            var col = interactable.GetComponent<Collider2D>();
            var closestPoint = col.ClosestPoint(transform.position);
            if (Vector2.Distance(transform.position, closestPoint) > _interactRange)
                continue;

            interactable.Interact();
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

    Vector2 RoundedMousePosition()
    {
        return Vector2Int.RoundToInt(
            _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue())
        );
    }
}