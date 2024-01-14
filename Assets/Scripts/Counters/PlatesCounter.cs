using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter {

  public event EventHandler OnPlateSpawned;
  public event EventHandler OnPlateRemoved;

  [SerializeField] private KitchenObject plateKitchenObjectSO;

  private float spawnPlateTimer;
  private float spawnPlateTimerMax = 4f;

  private int platesSpawned;
  private int platesSpawnedMax = 4;

  private void Update() {
    spawnPlateTimer += Time.deltaTime;
    if (spawnPlateTimer > spawnPlateTimerMax) {
      spawnPlateTimer = 0f;
      if (platesSpawned < platesSpawnedMax) {
        platesSpawned++;
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
      }
      //KitchenObjectMB.SpawnKitchenObject(plateKitchenObjectSO, this);
    }
  }

  public override void Interact(Player player) {
    if (!player.HasKitchenObject()) {
      // player is empty-handed
      if (platesSpawned > 0) {
        // Give plate to player
        platesSpawned--;
        KitchenObjectMB.SpawnKitchenObject(plateKitchenObjectSO, player);
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
      }
    }
  }

}