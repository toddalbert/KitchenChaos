using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
      public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.7f;
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    [SerializeField] private Transform kitchenObjectHoldPoint;

    private KitchenObjectMB kitchenObjectMB;

    private Vector3 lastInteractDir;

    private bool isWalking;

    private BaseCounter selectedCounter;

    private void Awake() {
      if (Instance != null) {
        Debug.LogError("There is more than one player Instance!");
      }
      Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAltAction += GameInput_OnInteractAltAction;
    }

  private void GameInput_OnInteractAltAction(object sender, EventArgs e) {
    if (selectedCounter != null) {
      selectedCounter.InteractAlternate(this);
    }
  }

  private void GameInput_OnInteractAction(object sender, EventArgs e) {

      if (selectedCounter != null) {
        selectedCounter.Interact(this);
      }
    }

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

      if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
        if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
          // Has ClearCounter
          if (baseCounter != selectedCounter) {
            SetSelectedCounter(baseCounter);
          }
        } else {
          SetSelectedCounter(null);
        }
      } else {
        SetSelectedCounter(null);
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
        canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
        if (canMove) {
          // can move only on X
          moveDir = moveDirX;
        } else {
          // Attempt only Z movement
          Vector3 moveDirZ = new Vector3(0, 0, moveDir.y).normalized;
          canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
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

    private void SetSelectedCounter(BaseCounter selectedCounter) {
      this.selectedCounter = selectedCounter;

      OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
        selectedCounter = selectedCounter
      });
    }


    public Transform GetKitchenObjectFollowTransform() {
      return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObjectMB kitchenObject) {
      this.kitchenObjectMB = kitchenObject;
    }

    public KitchenObjectMB GetKitchenObject() {
      return kitchenObjectMB;
    }

    public void ClearKitchenObject() {
      kitchenObjectMB = null;
    }

    public bool HasKitchenObject() {
      return kitchenObjectMB != null;
    }

}
