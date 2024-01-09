using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObjectMB : MonoBehaviour {

  [SerializeField] private KitchenObject kitchenObjectSO;

  private IKitchenObjectParent kitchenObjectParent;

  public KitchenObject GetKitchenObject() {
    return kitchenObjectSO;
  }

  public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
    if (this.kitchenObjectParent != null) {
      this.kitchenObjectParent.ClearKitchenObject();
    }

    this.kitchenObjectParent = kitchenObjectParent;
    if (kitchenObjectParent.HasKitchenObject()) {
      Debug.LogError("KitchenObjectParent already has a KitchenObject!");
    }
    kitchenObjectParent.SetKitchenObject(this);

    transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
    transform.localPosition = Vector3.zero;
  }

  public IKitchenObjectParent GetKitchenObjectParent() {
    return kitchenObjectParent;
  }
}
