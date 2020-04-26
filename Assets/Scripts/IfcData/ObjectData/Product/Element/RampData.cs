using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IRampData : IElementData
{
    string ShapeType { get; set; }
}

public class RampData : ElementData, IRampData
{
    private IIfcRamp thisRamp;
    private string shapeType;

    public string ShapeType { get => shapeType; set => shapeType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisRamp = ifcObj as IIfcRamp;

        shapeType = thisRamp.ShapeType.ToString();
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("ShapeType", shapeType);
    }
}
