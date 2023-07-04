using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType
{
    none = 0,

    //General
    goToHome = 100,
    goToWorkStation = 101,
    gotoWorkingPoint = 102,
    makingResource = 103,
    bringResourceBack = 104,
    findWorkingPoint = 105,
    getResNeedToMove = 106,

    //Woodcutter
    plantTree = 200,
    chopTree = 201,
    findTreeToChop = 102,//TODO: use findWorkingPoint instead

    //Warehouse
    findBuildingHasProduct = 300,
    goToGetProduct = 301,
    takingProductBack = 302,
    findBuildingNeedMaterial = 310,
    bringMaterialToBuilding = 311,

    //House Builder
    findWarehouseHasRes = 400,
    buildConstruction = 401,
}
