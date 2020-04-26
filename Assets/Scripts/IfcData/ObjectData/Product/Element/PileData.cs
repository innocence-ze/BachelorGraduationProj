using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IPileData : IElementData
{
    string PredefinedType { get; set; }
    string ConstructionType { get; set; }
}

public class PileData : ElementData, IPileData
{
    private IIfcPile thisPile;
    private string predefinedType;
    private string constructionType;

    public string PredefinedType { get => predefinedType; set => predefinedType = value; }
    public string ConstructionType { get => constructionType; set => constructionType = value; }

    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisPile = ifcObj as IIfcPile;

        predefinedType = thisPile.PredefinedType.ToString();
        if (thisPile.ConstructionType.HasValue)
            constructionType = thisPile.ConstructionType.Value.ToString();


        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("PredefinedType", predefinedType);
        generalProperties.Add("ConstructionType", constructionType);
    }
}
