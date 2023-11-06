using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EventCodeValues
{
    public enum EventCode : byte
    {
        START_GAME = 0,
        PICK_WINDOW = 1,
        BLUE_TURN =2,
        RED_TURN =3,
        READY =4,
        PICK_CHANGE =5,
        EXIT_GAME = 6
    }

}
