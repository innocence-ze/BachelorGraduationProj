using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IFlowTerminalData : IElementData
{
    string PredefinedType { get; set; }
}

public class FlowTerminalData : ElementData, IFlowTerminalData
{
    private IIfcFlowTerminal thisFlowTerminal;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisFlowTerminal = ifcObj as IIfcFlowTerminal;

        if (objType != null)
        {
            if (objType is IIfcLightFixtureType flowTerminalType)
                predefinedType = flowTerminalType.PredefinedType.ToString();
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
