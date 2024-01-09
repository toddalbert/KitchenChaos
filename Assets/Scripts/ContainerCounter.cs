using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {

  public event EventHandler OnPlayerGrabObject;

  [SerializeField] private KitchenObject kitchenObjectSO;

  public override void Interact(Player player) {
    if (!player.HasKitchenObject()) {
      Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
      kitchenObjectTransform.GetComponent<KitchenObjectMB>().SetKitchenObjectParent(player);

      OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
    }
  }

}