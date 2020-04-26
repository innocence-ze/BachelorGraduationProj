using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IBuildingElementProxyData : IElementData
{
    string CompositionType { get; set; }
}

public class BuildingElementProxyData : ElementData, IBuildingElementProxyData
{
    private IIfcBuildingElementProxy thisBEP;
    private string compositionType;

    public string CompositionType { get => compositionType; set => compositionType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisBEP = ifcObj as IIfcBuildingElementProxy;

        if (thisBEP.CompositionType.HasValue)
            compositionType = thisBEP.CompositionType.Value.ToString();
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("CompositionType", compositionType);
    }
}
