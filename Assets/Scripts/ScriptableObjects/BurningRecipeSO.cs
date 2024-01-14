using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class BurningRecipeSO : ScriptableObject {

  public KitchenObject input;
  public KitchenObject output;

  public float burningTimerMax;

}
