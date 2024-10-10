using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceLocked : MonoBehaviour
{
    public bool IsLocked;
    public bool AlreadyLocked;
    public Component Dice;
    public int DiceNumber;
    public Text Text;

    void Start()
    {
        IsLocked = false;
        AlreadyLocked = false;
    }

    void OnMouseUpAsButton()
    {
        if (AlreadyLocked)
            return;

        IsLocked = !IsLocked;

        Text.text = "Dice " + DiceNumber + ":";

        if (IsLocked)
            Text.text = "Dice " + DiceNumber + ": " + Dice.GetComponent<GuessDiceSide>().UpperSide;
    }
}
