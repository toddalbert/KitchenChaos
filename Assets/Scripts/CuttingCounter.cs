using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

  [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

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
      } else {
        // player not carrying anything
        GetKitchenObject().SetKitchenObjectParent(player);
      }
    }
  }

  public override void InteractAlternate(Player player) {
    if (HasKitchenObject()) {
      // Let's cut it
      KitchenObject outputKitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObject());
      GetKitchenObject().DestroySelf();

      KitchenObjectMB.SpawnKitchenObject(outputKitchenObject, this);
    }
  }

  private KitchenObject GetOutputForInput(KitchenObject inputKitchenObject) {
    foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
      if (cuttingRecipeSO.input == inputKitchenObject) {
        return cuttingRecipeSO.output;
      }
    }
    return null;
  }
}
