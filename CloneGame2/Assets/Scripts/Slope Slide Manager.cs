using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeSlideManager : MonoBehaviour
{
    [Header("Slope Settings")]
    [SerializeField] private float slopeLimit = 45f;
    [SerializeField] private float slideAcceleration = 5f;
    [SerializeField] private float maxSlideSpeed = 10f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float antiBumpFactor = 4f;

    private CharacterController _controller;
    private Vector3 _velocity;
    [SerializeField]private bool _isSliding;
    public bool CanSlide;

    void Awake() => _controller = GetComponent<CharacterController>();

    void Update()
    {
        if(CanSlide)
        {
            HandleSlopeSliding();
            ApplyMovement();
        }
    }

    private void HandleSlopeSliding()
    {
        
            if (IsOnSteepSlope(out Vector3 slopeNormal))
            {
                _isSliding = true;

                // Calculate slide direction (down the slope)
                Vector3 slideDirection = new Vector3(slopeNormal.x, -slopeNormal.y, slopeNormal.z);
                Vector3.OrthoNormalize(ref slopeNormal, ref slideDirection);

                // Accelerate down the slope
                _velocity += slideDirection * slideAcceleration * Time.deltaTime;

                // Clamp to max slide speed
                float horizontalSpeed = new Vector3(_velocity.x, 0, _velocity.z).magnitude;
                if (horizontalSpeed > maxSlideSpeed)
                {
                    _velocity = _velocity.normalized * maxSlideSpeed;
                    _velocity.y = -antiBumpFactor; // Prevent bouncing
                }
            }
            else
            {
                _isSliding = false;
                _velocity.y = -antiBumpFactor; // Small force to stay grounded
            }
        
        
    }

    private void ApplyMovement() => _controller.Move(_velocity * Time.deltaTime);

    private bool IsOnSteepSlope(out Vector3 slopeNormal)
    {
        if (_controller.isGrounded && Physics.Raycast(
            transform.position + _controller.center,
            Vector3.down,
            out RaycastHit hit,
            _controller.height / 2 + 0.5f))
        {
            slopeNormal = hit.normal;
            float slopeAngle = Vector3.Angle(slopeNormal, Vector3.up);
            return slopeAngle > _controller.slopeLimit && slopeAngle < 90f;
        }

        slopeNormal = Vector3.up;
        return false;
    }
}
