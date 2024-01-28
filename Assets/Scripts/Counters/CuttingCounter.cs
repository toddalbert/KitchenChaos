using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {

  public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

  public event EventHandler OnCut;

  [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

  private int cuttingProgress;

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      // can take an object
      if (player.HasKitchenObject()) {
        // transfer object to counter
        if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObject())) {
          // Player has something that can be cut
          player.GetKitchenObject().SetKitchenObjectParent(this);
          cuttingProgress = 0;
          CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObject());

          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingCount
          });
        }
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

  public override void InteractAlternate(Player player) {
    if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObject())) {
      // Let's cut it

      cuttingProgress++;

      OnCut?.Invoke(this, EventArgs.Empty);

      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObject());

      OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
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
