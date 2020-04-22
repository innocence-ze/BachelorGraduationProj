using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
public interface IBuildingData: ISpatialData
{

}
public class BuildingData : SpatialData, IBuildingData
{
    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);

        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.spatialStructures.Add(this);
    }
}
