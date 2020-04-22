using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces; 
public interface ISpaceData : ISpatialData
{
    string Elevation { get; set; }
} 
public class SpaceData : SpatialData, ISpaceData
{
    IIfcSpace thisSpace;
    string elevation;

    public string Elevation { get => elevation; set => elevation = value; }


    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);

        thisSpace = ifcObj as IIfcSpace;
        if (thisSpace.ElevationWithFlooring.HasValue)
            elevation = thisSpace.ElevationWithFlooring.Value.ToString();
        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.spatialStructures.Add(this);
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("Elevation", elevation);
    }
}
