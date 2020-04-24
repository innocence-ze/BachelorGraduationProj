using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
public interface IElementData : IProductData
{
    bool HasOpening { get; set; }
    string Tag { get; set; }
    IIfcElement ThisElement { get; set; }

    string StyleName { get; set; }
    string StyleDescription { get; set; }

    void SetDecomposeProduct(IEnumerable<IIfcRelDecomposes> relatedProd);
}
[Serializable]
public class ElementData : ProductData, IElementData
{
    private IIfcElement thisElement;
    private List<IIfcElement> openingEles = new List<IIfcElement>();

    private string elementTag;
    private bool hasOpening = false;

    private string styleName;
    private string styleDescription;

    public bool HasOpening { get => hasOpening; set => hasOpening = value; }
    public IIfcElement ThisElement { get => thisElement; set => thisElement = value; }
    public string Tag { get => elementTag; set => elementTag = value; }

    public string StyleName { get => styleName; set => styleName = value; }
    public string StyleDescription { get => styleDescription; set => styleDescription = value; }


    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisElement = ifcObj as IIfcElement;
        //IIfcRelDefinesByProperties
        elementTag = thisElement.Tag;
        foreach(var o in thisElement.HasOpenings)
        {
            openingEles.Add(o.RelatedOpeningElement);
        }
        if (openingEles.Count > 0)
        {
            hasOpening = true;
        }

        if (objType != null)
        {
            if (objType.Name.HasValue)
                styleName = objType.Name.Value.ToString();
            if (objType.Description.HasValue)
                styleDescription = objType.Description.Value.ToString();
        }

        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.Elements.Add(this);
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("Tag", elementTag);
        generalProperties.Add("HasOpening", hasOpening.ToString());
        if(objType != null)
        {
            generalProperties.Add("StyleName", styleName);
            generalProperties.Add("StyleDescription", styleDescription);
        }
    }

    public void SetDecomposeProduct(IEnumerable<IIfcRelDecomposes> connects)
    {
        foreach(var c in connects)
        {
            foreach(var prod in c.RelatedObjects)
            {
                var ele = BimReader.InstantiateCurElement(prod as IIfcElement);
                AddRelatedObjects(ele);
            }
        }
    }
}
