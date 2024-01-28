using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {

  public enum State {
    Idle,
    Frying,
    Fried,
    Burned,
  }

  public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
  public class OnStateChangedEventArgs : EventArgs {
    public State currentState;
  }

  public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

  [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
  [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

  private State currentState;

  private float fryingTimer = 0f;
  private float burningTimer = 0f;
  private FryingRecipeSO fryingRecipeSO;
  private BurningRecipeSO burningRecipeSO;

  private void Start() {
    currentState = State.Idle;
  }

  private void Update() {

    if (HasKitchenObject()) {
      switch (currentState) {
        case State.Idle:
          break;
        case State.Frying:
          fryingTimer += Time.deltaTime;

          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
          });

          if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
            // Fried
            GetKitchenObject().DestroySelf();
            KitchenObjectMB.SpawnKitchenObject(fryingRecipeSO.output, this);
            burningRecipeSO = GetBurningRecipeSOWithInput(fryingRecipeSO.output);
            currentState = State.Fried;
            burningTimer = 0f;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
              currentState = currentState
            });
          }
          break;
        case State.Fried:
          burningTimer += Time.deltaTime;

          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
          });

          if (burningTimer > burningRecipeSO.burningTimerMax) {
            // Fried
            GetKitchenObject().DestroySelf();
            KitchenObjectMB.SpawnKitchenObject(burningRecipeSO.output, this);
            currentState = State.Burned;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
              currentState = currentState
            });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
              progressNormalized = 0f
            });
          }
          break;
        case State.Burned:
          break;
      }
    }

  }

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      // can take an object
      if (player.HasKitchenObject()) {
        // transfer object to counter
        if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObject())) {
          // Player has something that can be fried
          player.GetKitchenObject().SetKitchenObjectParent(this);
          fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObject());

          currentState = State.Frying;
          fryingTimer = 0f;

          OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
            currentState = currentState
          });

          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
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

            currentState = State.Idle;

            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
              currentState = currentState
            });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
              progressNormalized = 0f
            });
          }
      } else {
        // player not carrying anything
        GetKitchenObject().SetKitchenObjectParent(player);
        currentState = State.Idle;

        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
          currentState = currentState
        });

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
          progressNormalized = 0f
        });
      }
    }
  }



  private bool HasRecipeWithInput(KitchenObject inputKitchenObject) {
    FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObject);

    return fryingRecipeSO != null;
  }

  private KitchenObject GetOutputForInput(KitchenObject inputKitchenObject) {
    FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObject);
    if (fryingRecipeSO != null) {
      return fryingRecipeSO.output;
    } else {
      return null;
    }
  }

  private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObject inputKitchenObject) {
    foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
      if (fryingRecipeSO.input == inputKitchenObject) {
        return fryingRecipeSO;
      }
    }
    return null;
  }

  private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObject inputKitchenObject) {
    foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
      if (burningRecipeSO.input == inputKitchenObject) {
        return burningRecipeSO;
      }
    }
    return null;
  }


}