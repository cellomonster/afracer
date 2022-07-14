using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Steer : MonoBehaviour
{
    [SerializeField] private float torque = 20;

    [SerializeField] private new Rigidbody rigidbody;

    public float Power;

    public void SetPower(float power)
    {
        this.Power = Mathf.Clamp(power, -1, 1);
    }

    public void SetPower(InputAction.CallbackContext context)
    {
        SetPower(context.ReadValue<Vector2>().x);
    }

    private void FixedUpdate()
    {
        if (Power == 0)
            return;

        rigidbody.AddRelativeTorque(new Vector3(0, torque * Power, 0));
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();
    }
#endif
}
