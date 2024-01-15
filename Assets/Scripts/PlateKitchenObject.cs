using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObjectMB {

  private List<KitchenObject> kitchenObjectList;

  public void Awake() {
    kitchenObjectList = new List<KitchenObject>();
  }

  public void AddIngredient(KitchenObject kitchenObject) {
    kitchenObjectList.Add(kitchenObject);
  }
}
