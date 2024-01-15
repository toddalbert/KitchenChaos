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
        if (player.GetKitchenObject() is PlateKitchenObject) {
          // player is holding a plate
          PlateKitchenObject plateKitchenObject = player.GetKitchenObject() as PlateKitchenObject;
          plateKitchenObject.AddIngredient(GetKitchenObject().GetKitchenObject());
          GetKitchenObject().DestroySelf();
        }
      } else {
        // player not carrying anything
        GetKitchenObject().SetKitchenObjectParent(player);
      }
    }
  }

}
