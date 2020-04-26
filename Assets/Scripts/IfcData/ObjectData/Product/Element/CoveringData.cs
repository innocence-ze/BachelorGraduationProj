using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface ICoveringData : IElementData
{
    string PredefinedType { get; set; }
}

public class CoveringData : ElementData, ICoveringData
{
    private IIfcCovering thisCovering;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisCovering = ifcObj as IIfcCovering;

        if (thisCovering.PredefinedType.HasValue)
            predefinedType = thisCovering.PredefinedType.Value.ToString();
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
