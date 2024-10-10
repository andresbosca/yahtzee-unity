using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessDiceSide : MonoBehaviour
{
    public Vector3Int DirectionValues;
    private Vector3Int OpposingDirectionValues;

    readonly List<int> FaceRepresent = new() { 0, 1, 2, 3, 4, 5, 6 };

    public int UpperSide;

    // Start is called before the first frame update
    void Start()
    {
        OpposingDirectionValues = 7 * Vector3Int.one - DirectionValues;
        transform.Rotate(Vector3.up, Random.value * 90);
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.hasChanged)
        {
            return;
        }

        SetUpperSide();

        transform.hasChanged = false;
    }

    private void SetUpperSide()
    {
        if ((int)Vector3.Cross(Vector3.up, transform.right).magnitude == 0)
        {
            if (Vector3.Dot(Vector3.up, transform.right) > 0)
            {
                UpperSide = FaceRepresent[DirectionValues.x];
                return;
            }

            UpperSide = FaceRepresent[OpposingDirectionValues.x];
            return;
        }
        
        if ((int)Vector3.Cross(Vector3.up, transform.up).magnitude == 0)
        {
            if (Vector3.Dot(Vector3.up, transform.up) > 0)
            {
                UpperSide = FaceRepresent[DirectionValues.y];
                return;
            }

            UpperSide = FaceRepresent[OpposingDirectionValues.y];
            return;
        }

        if ((int)Vector3.Cross(Vector3.up, transform.forward).magnitude == 0)
        {
            if (Vector3.Dot(Vector3.up, transform.forward) > 0)
            {
                UpperSide = FaceRepresent[DirectionValues.z];
                return;
            }

            UpperSide = FaceRepresent[OpposingDirectionValues.z];
            return;
        }
    }
}
