using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelAnimation : MonoBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;

    [Header("Turn banking")]

    [SerializeField] private float bankMaxAngle = 20f;
    [SerializeField] private float bankLerpStrength = 0.5f;

    private float _steer;

    public void Steer(InputAction.CallbackContext context)
    {
        _steer = context.ReadValue<Vector2>().x;
    }

    private void Update()
    {
        float bankAngle = _steer * bankMaxAngle * -1;
        Quaternion targetRotation = Quaternion.Euler(0, 0, bankAngle);

        float lerpTime = bankLerpStrength * Time.deltaTime * 60;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, lerpTime);
    }
}
