using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent {


  [SerializeField] private KitchenObject kitchenObjectSO;
  [SerializeField] private Transform counterTopPoint;

  private KitchenObjectMB kitchenObjectMB;


  public void Interact(Player player) {
    if (kitchenObjectMB == null) {
      // if no object, spawn it:
      Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
      kitchenObjectTransform.GetComponent<KitchenObjectMB>().SetKitchenObjectParent(this);
    } else {
      // if object on counter, give it to player
      kitchenObjectMB.SetKitchenObjectParent(player);
    }
  }

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
