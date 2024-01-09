using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {

  [SerializeField] private KitchenObject kitchenObjectSO;

  public override void Interact(Player player) {
    Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
    kitchenObjectTransform.GetComponent<KitchenObjectMB>().SetKitchenObjectParent(player);
  }

}