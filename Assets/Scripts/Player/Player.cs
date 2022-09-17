using System;
using Animancer;
using FarrokhGames.Inventory;
using UnityEngine;

[RequireComponent(typeof(AnimancerComponent))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IPersistableData
{
    [SerializeField] float _speed = 7;

    [SerializeField] PlayerAudio _audio;
    [SerializeField] PlayerController _controller;
    [SerializeField] Animations _animations;

    public InventoryManager inventory { get; private set; }

    AnimancerComponent _animancer;
    Rigidbody2D _body;

    Vector2Int _facingDirection = Vector2Int.down;

    void Awake()
    {
        _animancer = GetComponent<AnimancerComponent>();
        _body = GetComponent<Rigidbody2D>();

        var provider = new InventoryProvider();
        inventory = new InventoryManager(provider, 4, 4);
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
        inventory.TryAdd(collectible.itemObject.CreateInstance());
    }

    void UpdateMovement()
    {
        var moveInput = _controller.actions.Move.ReadValue<Vector2>();
        var movement = _speed * Time.fixedDeltaTime * moveInput;

        _body.MovePosition((Vector2)transform.position + movement);
    }

    void UpdateDirection()
    {
        var moveInput = _controller.actions.Move.ReadValue<Vector2>();
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
        var moveInput = _controller.actions.Move.ReadValue<Vector2>();
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