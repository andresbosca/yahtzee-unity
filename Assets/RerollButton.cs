using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DiceManager;

public class RerollButton : MonoBehaviour
{
    public Component Dice1;
    public Component Dice2;
    public Component Dice3;
    public Component Dice4;
    public Component Dice5;
    public int RerollCount;

    private void Start()
    {

    }
    private void OnMouseUpAsButton()
    {
        if (RerollCount == 3)
            return;

        Dice1.TryGetComponent<DiceLocked>(out var lockedDice1);
        Dice2.TryGetComponent<DiceLocked>(out var lockedDice2);
        Dice3.TryGetComponent<DiceLocked>(out var lockedDice3);
        Dice4.TryGetComponent<DiceLocked>(out var lockedDice4);
        Dice5.TryGetComponent<DiceLocked>(out var lockedDice5);

        if (lockedDice1.IsLocked &&
            lockedDice2.IsLocked &&
            lockedDice3.IsLocked &&
            lockedDice4.IsLocked &&
            lockedDice5.IsLocked)
        {
            RerollCount++;
            return;
        }

        if (!lockedDice1.IsLocked)
        {
            Dice1.transform.position = new Vector3(0, 18, -1);
            Dice1.transform.Rotate(Vector3.up, 60);
            Dice1.transform.Rotate(Vector3.left, 60);
        }

        if (!lockedDice2.IsLocked)
        {
            Dice2.transform.position = new Vector3(2, 16, -1);
            Dice2.transform.Rotate(Vector3.up, 75);
            Dice2.transform.Rotate(Vector3.left, 75);
        }

        if (!lockedDice3.IsLocked)
        {
            Dice3.transform.position = new Vector3(4, 20, -1);
            Dice3.transform.Rotate(Vector3.up, 75);
            Dice3.transform.Rotate(Vector3.left, 75);
        }

        if (!lockedDice4.IsLocked)
        {
            Dice4.transform.position = new Vector3(-2, 17, -1);
            Dice4.transform.Rotate(Vector3.up, 75);
            Dice4.transform.Rotate(Vector3.left, 75);
        }

        if (!lockedDice5.IsLocked)
        {
            Dice5.transform.position = new Vector3(-4, 19, -1);
            Dice5.transform.Rotate(Vector3.up, 75);
            Dice5.transform.Rotate(Vector3.left, 75);
        }
        RerollCount++;
    }
}
