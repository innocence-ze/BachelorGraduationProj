using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IPlateData : IElementData
{
    string PredefinedType { get; set; }
}

public class PlateData : ElementData, IPlateData
{
    private IIfcPlate thisPlate;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisPlate = ifcObj as IIfcPlate;

        if (objType != null)
        {
            IIfcPlateType plateType = objType as IIfcPlateType;
            predefinedType = plateType.PredefinedType.ToString();
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
