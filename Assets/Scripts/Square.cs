using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public enum SquareStates
    {
        None,
        X, 
        O
    }
    public SquareStates state = SquareStates.None;
}
