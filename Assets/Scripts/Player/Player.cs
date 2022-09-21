using System;
using Animancer;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IPersistableData
{
    [SerializeField] float _speed = 7;

    [SerializeField] PlayerAudio _audio;
    [SerializeField] PlayerController _controller;
    [SerializeField] PlayerOxygen _oxygen;

    [SerializeField] Animations _animations;

    public PlayerController controller => _controller;
    public PlayerInventory inventory { get; private set; }
    public PlayerOxygen oxygen => _oxygen;

    AnimancerComponent _animancer;
    Rigidbody2D _body;

    Vector2Int _facingDirection = Vector2Int.down;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _body = GetComponent<Rigidbody2D>();
        inventory = new PlayerInventory(this);
    }

    void FixedUpdate()
    {
        UpdateMovement();
        UpdateDirection();
        // UpdateAnimations();
    }

    public void Load(PersistentData data)
    {
        if (data.player == null) return;

        transform.position = data.player.position;
        SetFacingDirection(data.player.facingDirection);
    }

    public void Save(PersistentData data)
    {
        data.player = new Data
        {
            position = transform.position,
            facingDirection = _facingDirection,
        };
    }

    public void Footstep()
    {
        _audio.Footstep();
    }

    public void Collect(Collectible collectible)
    {
        inventory.bag.TryAdd(collectible.item.CreateInstance());
    }

    void UpdateMovement()
    {
        var moveInput = _controller.MoveValue();
        var movement = _speed * Time.fixedDeltaTime * moveInput;

        _body.MovePosition((Vector2)transform.position + movement);
    }

    void UpdateDirection()
    {
        var moveInput = _controller.MoveValue();
        if (moveInput == default || moveInput == _facingDirection)
            return;

        SetFacingDirection(
            moveInput.y == 0
                ? Vector2Int.RoundToInt(moveInput)
                : new Vector2Int(0, Mathf.RoundToInt(moveInput.y))
        );
    }

    void SetFacingDirection(Vector2Int facingDirection)
    {
        _facingDirection = facingDirection;

        if ((_facingDirection.x < 0 && transform.localScale.x > 0) ||
            (_facingDirection.x > 0 && transform.localScale.x < 0))
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    void UpdateAnimations()
    {
        var directionalAnimationSet = GetDirectionalAnimationSet();
        _animancer.Play(directionalAnimationSet.GetClip(_facingDirection));
    }

    DirectionalAnimationSet GetDirectionalAnimationSet()
    {
        var moveInput = _controller.MoveValue();
        return moveInput == default ? _animations.idle : _animations.walk;
    }

    [Serializable]
    class Animations
    {
        public DirectionalAnimationSet idle;
        public DirectionalAnimationSet walk;
    }

    public class Data
    {
        public Vector3 position;
        public Vector2Int facingDirection;
    }
}