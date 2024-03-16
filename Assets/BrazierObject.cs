using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrazierObject : MonoBehaviour
{
    public Dictionary<int,InvenItem> brazier = new Dictionary<int,InvenItem>();
    public Slot fuelSlot, resourceSlot, finishedSlot;
    public Image fill_ImageUI;

    public void InIt()
    {
    }
}
