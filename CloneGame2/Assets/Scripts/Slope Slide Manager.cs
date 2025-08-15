using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SlopeSlideManager : MonoBehaviour
{
    [Header("Force Settings")]
    public float forcePower = 10f;
    public bool CanSlide;
    private CharacterController _controller;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if(CanSlide)
        {
            AddBackwardForce();
            forcePower = 0.5f;

        }
        else if(!CanSlide)
        {
            forcePower = 0;
        }
    }
    // Call this to apply instant backward force
    public void AddBackwardForce()
    {
        Vector3 force = -transform.forward * forcePower * Time.deltaTime;
        _controller.Move(force);
    }

    // Call this to apply force in any direction
    public void AddForce(Vector3 direction, float power)
    {
        _controller.Move(direction.normalized * power);
    }
}