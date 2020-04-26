using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IFurnitureData : IElementData
{
    string AssemblyPlace { get; set; }
}

public class FurnitureData : ElementData
{
    private IIfcFurnishingElement thisFurniture;
    private string assemblyPlace;

    public string AssemblyPlace { get => assemblyPlace; set => assemblyPlace = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisFurniture = ifcObj as IIfcFurnishingElement;

        if (objType != null)
        {
            if (objType is IIfcFurnitureType furnitureType)
                assemblyPlace = furnitureType.AssemblyPlace.ToString();
        }
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        if (objType != null)
        {
            generalProperties.Add("AssemblyPlace", assemblyPlace);
        }
    }
}
