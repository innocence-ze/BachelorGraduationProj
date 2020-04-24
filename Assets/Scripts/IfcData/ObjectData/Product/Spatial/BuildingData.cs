using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
public interface IBuildingData: ISpatialData
{
    string ElevationOfHeight { get; set; }
    string ElevationOfTerrain { get; set; }
}
public class BuildingData : SpatialData, IBuildingData
{
    private IIfcBuilding thisBuilding;
    private string elevationOfHeight;
    private string elevationOfTerrain;

    public string ElevationOfHeight { get => elevationOfHeight; set => elevationOfHeight = value; }
    public string ElevationOfTerrain { get => elevationOfTerrain; set => elevationOfTerrain = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);

        thisBuilding = ifcObj as IIfcBuilding;

        if (thisBuilding.ElevationOfRefHeight.HasValue)
            elevationOfHeight = thisBuilding.ElevationOfRefHeight.Value.ToString();
        if (thisBuilding.ElevationOfTerrain.HasValue)
            elevationOfTerrain = thisBuilding.ElevationOfTerrain.Value.ToString();

        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.spatialStructures.Add(this);
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("ElevationOfHeight", elevationOfHeight);
        generalProperties.Add("ElevationOfTerrain", elevationOfTerrain);
    }
}
