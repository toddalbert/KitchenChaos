using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObjectMB {

  public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
  public class OnIngredientAddedEventArgs : EventArgs {
    public KitchenObject kitchenObject;
  }

  [SerializeField] private List<KitchenObject> validKitchenObjectList;

  private List<KitchenObject> kitchenObjectList;

  public void Awake() {
    kitchenObjectList = new List<KitchenObject>();
  }

  public bool TryAddIngredient(KitchenObject kitchenObject) {
    if (!validKitchenObjectList.Contains(kitchenObject)) {
      return false; // not a valid ingredient
    }
    if (kitchenObjectList.Contains(kitchenObject)) {
      return false; // already contains this ingredient
    }
    kitchenObjectList.Add(kitchenObject); // add new item to plate in hand
    OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObject = kitchenObject });
    return true;
  }
}
