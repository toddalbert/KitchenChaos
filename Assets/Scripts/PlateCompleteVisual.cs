using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {

    [Serializable]
    public struct KitchenObject_GameObject {
        public KitchenObject kitchenObject;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObject_GameObject> kitchenObject_GameObjectList;

    private void Start() {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
        foreach (KitchenObject_GameObject kitchenObject_GameObject in kitchenObject_GameObjectList) {
            kitchenObject_GameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
        foreach (KitchenObject_GameObject kitchenObject_GameObject in kitchenObject_GameObjectList) {
            if (kitchenObject_GameObject.kitchenObject == e.kitchenObject) {
                kitchenObject_GameObject.gameObject.SetActive(true);
            }
        }
    }


}
