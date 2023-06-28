using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    none = 0,

    //Going to
    goToWorkStation = 1,
    goToHome = 2,

    //Woodcutter
    plantTree = 100,
    chopTree = 101,
    findTreeToChop = 102,

    bringResourceBack = 1000,
    makingResource = 1001,
    gotoWorkingPoint = 1002,
    getResNeedToMove = 1003,
    //Smith
}
