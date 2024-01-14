using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]

public class FryingRecipeSO : ScriptableObject {

  public KitchenObject input;
  public KitchenObject output;

  public float fryingTimerMax;

}
