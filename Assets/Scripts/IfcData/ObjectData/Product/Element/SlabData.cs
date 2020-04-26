using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface ISlabData : IElementData
{
    string PredefinedType { get; set; }
}

public class SlabData : ElementData, ISlabData
{
    private IIfcSlab thisSlab;
    private string predefinedType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisSlab = ifcObj as IIfcSlab;

        if (objType != null)
        {
            IIfcSlabType slabType = objType as IIfcSlabType;
            predefinedType = slabType.PredefinedType.ToString();
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
