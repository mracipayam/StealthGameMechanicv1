using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public float moveSpeed = 7f;
   public float smoothMoveTime = .1f;
   public float turnSpeed = 8f;
   float smoothInputMagnitude;
   float smoothMoveVelocity;
   private float angle;
   private Rigidbody _rigidbody;
   private Vector3 velocity;
   private bool disabled;

   private void Start()
   {
      _rigidbody = GetComponent<Rigidbody>();
      Guard.OnGuardHasSpottedPlayer += Disable;
   }

   private void Update()
   {
      Vector3 inputDirection = Vector3.zero;
      if (!disabled)
      {
         inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
      }
      float inputMagnitude = inputDirection.magnitude;
      smoothInputMagnitude =
         Mathf.SmoothDamp(smoothInputMagnitude, inputMagnitude, ref smoothMoveVelocity, smoothMoveTime);
      float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
      angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * inputMagnitude);
      transform.eulerAngles = Vector3.up * angle;
      transform.Translate(transform.forward * moveSpeed * Time.deltaTime * smoothInputMagnitude,Space.World);

      velocity = transform.forward * moveSpeed * smoothInputMagnitude;
   }

   void Disable()
   {
      disabled = true;
   }

   private void FixedUpdate()
   {
      _rigidbody.MoveRotation(Quaternion.Euler(Vector3.up * angle));
      _rigidbody.MovePosition(_rigidbody.position + velocity * Time.deltaTime);
   }

   private void OnDestroy()
   {
      Guard.OnGuardHasSpottedPlayer -= Disable;
   }
}
