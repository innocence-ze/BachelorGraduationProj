using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
public interface IBuildingStoreyData : ISpatialData
{
    string Elevation { get; set; }
}
public class BuildingStoreyData : SpatialData, IBuildingStoreyData
{
    private IIfcBuildingStorey thisBuildingStorey;
    private string elevation;

    public string Elevation { get => elevation; set => elevation = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisBuildingStorey = ifcObj as IIfcBuildingStorey;
        if (thisBuildingStorey.Elevation.HasValue)
            elevation = thisBuildingStorey.Elevation.Value.ToString();
        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.spatialStructures.Add(this);
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("Elevation", elevation);
    }

}

