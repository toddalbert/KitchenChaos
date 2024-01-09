using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

  [SerializeField] private KitchenObject kitchenObjectSO;

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      // can take an object
      if (player.HasKitchenObject()) {
        // transfer object to counter
        player.GetKitchenObject().SetKitchenObjectParent(this);
      }
    } else {
      // spot is already taken
    }
  }

}
