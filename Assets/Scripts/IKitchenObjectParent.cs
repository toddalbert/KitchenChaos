using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent {

  public Transform GetKitchenObjectFollowTransform();

  public void SetKitchenObject(KitchenObjectMB kitchenObject);

  public KitchenObjectMB GetKitchenObject();

  public void ClearKitchenObject();

  public bool HasKitchenObject();

}