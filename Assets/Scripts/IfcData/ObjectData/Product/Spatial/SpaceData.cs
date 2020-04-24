using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces; 
public interface ISpaceData : ISpatialData
{
    string ElevationWithFloor { get; set; }
    string SpaceType { get; set; }
} 
public class SpaceData : SpatialData, ISpaceData
{
    IIfcSpace thisSpace;
    string elevationWithFloor;
    string spaceType;

    public string ElevationWithFloor { get => elevationWithFloor; set => elevationWithFloor = value; }
    public string SpaceType { get => spaceType; set => spaceType = value; }


    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);

        thisSpace = ifcObj as IIfcSpace;
        
        if (thisSpace.ElevationWithFlooring.HasValue)
            elevationWithFloor = thisSpace.ElevationWithFlooring.Value.ToString();
        spaceType = thisSpace.InteriorOrExteriorSpace.ToString();
        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.spatialStructures.Add(this);
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("ElevationWithFloor", elevationWithFloor);
        generalProperties.Add("SpaceType", spaceType);
    }
}
