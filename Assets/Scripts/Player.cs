using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

  [SerializeField] private float moveSpeed = 7f;
  [SerializeField] private float rotateSpeed = 10f;
  [SerializeField] private float playerHeight = 2f;
  [SerializeField] private float playerRadius = 0.7f;
  [SerializeField] private float interactDistance = 2f;
  [SerializeField] private GameInput gameInput;

  private Vector3 lastInteractDir;

  private bool isWalking;

  private void Update() {
    HandleMovement();
    HandleInteractions();
  }

  public bool IsWalking() {
    return isWalking;
  }

  private void HandleInteractions() {

    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

    if (moveDir != Vector3.zero) {
      lastInteractDir = moveDir;
    }

    if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance)) {
      Debug.Log(raycastHit.transform);
    }

  }

  private void HandleMovement() {

    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
    float moveDistance = moveSpeed * Time.deltaTime;

    bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

    isWalking = moveDir != Vector3.zero;

    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    if (!canMove) {
      // cannot move towards moveDir
      // Attempt only X movement
      Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
      canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
      if (canMove) {
        // can move only on X
        moveDir = moveDirX;
      } else {
        // Attempt only Z movement
        Vector3 moveDirZ = new Vector3(0, 0, moveDir.y).normalized;
        canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
        if (canMove) {
          // can move only on Z
          moveDir = moveDirZ;
        }
      }
    }

    if (canMove) {
      transform.position += moveDir * moveDistance;
    }
  }

}
