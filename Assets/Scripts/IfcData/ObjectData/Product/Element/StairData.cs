using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IStairData : IElementData
{
    string ShapeType { get; set; }
}

public class StairData : ElementData, IStairData
{
    private IIfcStair thisStair;
    private string shapeType;

    public string ShapeType { get => shapeType; set => shapeType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisStair = ifcObj as IIfcStair;

        shapeType = thisStair.ShapeType.ToString();

        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("ShapeType", shapeType);
    }
}
