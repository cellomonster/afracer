using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class CenterOfMass : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Vector3 centerOfMass;

        private void Awake()
        {
            rigidbody.centerOfMass = centerOfMass;
        }
    }
}