using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IRoofData : IElementData
{
    string ShapeType { get; set; }
}

public class RoofData : ElementData, IRoofData
{
    private IIfcRoof thisRoof;
    private string shapeType;

    public string ShapeType { get => shapeType; set => shapeType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisRoof = ifcObj as IIfcRoof;

        shapeType = thisRoof.ShapeType.ToString();
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {    
        generalProperties.Add("ShapeType", shapeType);
    }
}
