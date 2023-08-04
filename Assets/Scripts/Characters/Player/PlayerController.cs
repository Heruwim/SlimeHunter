using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 1f;
    [SerializeField] private float _collisionOffset = 0.05f;
    [SerializeField] private ContactFilter2D _movementFilter2D;
    
    private Vector2 _movementInput;
    private Rigidbody2D _playerRigidbody2D;
    private  List<RaycastHit2D> _castCollisions = new List<RaycastHit2D>();

    private void Start()
    {
        _playerRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_movementInput != Vector2.zero)
        {
            bool success = TryMove(_movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(_movementInput.x, 0));

                if (!success)
                {
                    success = TryMove(new Vector2(0, _movementInput.y));
                }
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        int count = _playerRigidbody2D.Cast(
            direction,
            _movementFilter2D,
            _castCollisions,
            _movementSpeed * Time.fixedDeltaTime + _collisionOffset);

        if (count == 0)
        {
            _playerRigidbody2D.MovePosition(_playerRigidbody2D.position + direction * _movementSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        return false;
    }

    private void OnMove(InputValue movementValue)
    {
        _movementInput = movementValue.Get<Vector2>();
    }
}
