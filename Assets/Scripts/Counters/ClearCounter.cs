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
      } else {
        // player not carrying anything
      }
    } else {
      // spot is already taken
      if (player.HasKitchenObject()) {
        // player is carrying something
        if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
          // player is holding a plate
          if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObject())) {
            // successfully added ingredient to plate
            GetKitchenObject().DestroySelf();
          }
        }
      } else {
        // player not carrying anything
        GetKitchenObject().SetKitchenObjectParent(player);
      }
    }
  }

}
