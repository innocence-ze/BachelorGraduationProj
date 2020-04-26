using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IFootingData : IElementData
{
    string PredefinedType { get; set; }
}

public class FootingData : ElementData, IFootingData
{
    private IIfcFooting thisFooting;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisFooting = ifcObj as IIfcFooting;
        predefinedType = thisFooting.PredefinedType.ToString();

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
