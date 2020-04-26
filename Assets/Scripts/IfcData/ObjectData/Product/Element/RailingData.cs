using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IRailingData : IElementData
{
    string PredefinedType { get; set; }
}

public class RailingData : ElementData, IRailingData
{
    private IIfcRailing thisRailing;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisRailing = ifcObj as IIfcRailing;

        if(thisRailing.PredefinedType.HasValue)
            predefinedType = thisRailing.PredefinedType.Value.ToString();

        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("PredefinedType", predefinedType);
    }
}
