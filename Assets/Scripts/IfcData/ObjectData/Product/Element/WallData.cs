using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IWallData
{
    string PredefinedType { get; set; }
}

public class WallData : ElementData, IWallData
{
    private IIfcWall thisWall;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisWall = ifcObj as IIfcWall;

        if (objType != null)
        {
            IIfcWallType wallType = objType as IIfcWallType;
            predefinedType = wallType.PredefinedType.ToString();
        }
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        if (objType != null)
        {
            generalProperties.Add("PredefinedType", predefinedType);
        }
    }
}
