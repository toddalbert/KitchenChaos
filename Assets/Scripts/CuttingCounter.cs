using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

  public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

  public class OnProgressChangedEventArgs : EventArgs {
    public float progressNormalized;
  }

  public event EventHandler OnCut;

  [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

  private int cuttingProgress;

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      // can take an object
      if (player.HasKitchenObject()) {
        // transfer object to counter
        player.GetKitchenObject().SetKitchenObjectParent(this);
        cuttingProgress = 0;
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObject());

        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
          progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingCount
        });
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
    if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObject())) {
      // Let's cut it

      cuttingProgress++;

      OnCut?.Invoke(this, EventArgs.Empty);

      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObject());

      OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingCount
      });

      if (cuttingProgress >= cuttingRecipeSO.cuttingCount) {
        KitchenObject outputKitchenObject = GetOutputForInput(GetKitchenObject().GetKitchenObject());
        GetKitchenObject().DestroySelf();

        KitchenObjectMB.SpawnKitchenObject(outputKitchenObject, this);
      }


    }
  }

  private bool HasRecipeWithInput(KitchenObject inputKitchenObject) {
    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObject);

    return cuttingRecipeSO != null;
  }

  private KitchenObject GetOutputForInput(KitchenObject inputKitchenObject) {
    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObject);
    if (cuttingRecipeSO != null) {
      return cuttingRecipeSO.output;
    } else {
      return null;
    }
  }

  private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObject inputKitchenObject) {
    foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
      if (cuttingRecipeSO.input == inputKitchenObject) {
        return cuttingRecipeSO;
      }
    }
    return null;
  }


}
