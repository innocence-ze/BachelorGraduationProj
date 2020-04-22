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
    void SetDecomposeProduct(IEnumerable<IIfcRelDecomposes> relatedProd);
}
[Serializable]
public class ElementData : ProductData, IElementData
{
    private IIfcElement thisElement;
    private List<IIfcElement> openingEles = new List<IIfcElement>();

    [SerializeField]
    private string elementTag;
    [SerializeField]
    private bool hasOpening = false;

    public bool HasOpening { get => hasOpening; set => hasOpening = value; }
    public IIfcElement ThisElement { get => thisElement; set => thisElement = value; }
    public string Tag { get => elementTag; set => elementTag = value; }


    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisElement = ifcObj as IIfcElement;

        elementTag = thisElement.Tag;
        foreach(var o in thisElement.HasOpenings)
        {
            openingEles.Add(o.RelatedOpeningElement);
        }
        if (openingEles.Count > 0)
        {
            hasOpening = true;
        }

        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.Elements.Add(this);
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("Tag", elementTag);
        generalProperties.Add("HasOpening", hasOpening.ToString());
    }

    public void SetDecomposeProduct(IEnumerable<IIfcRelDecomposes> connects)
    {
        foreach(var c in connects)
        {
            foreach(var prod in c.RelatedObjects)
            {
                var go = new GameObject();
                var ele = go.AddComponent<ElementData>();
                ele.InitialObject(prod as IIfcObject);
                AddRelatedObjects(ele);
            }
        }
    }
}
