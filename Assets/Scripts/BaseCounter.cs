using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {


  [SerializeField] private Transform counterTopPoint;

  private KitchenObjectMB kitchenObjectMB;

  public virtual void Interact(Player player) { }

  public virtual void InteractAlternate(Player player) { }


  public Transform GetKitchenObjectFollowTransform() {
    return counterTopPoint;
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
