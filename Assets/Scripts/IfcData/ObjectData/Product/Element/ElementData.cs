using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
public interface IElementData : IProductData
{
    bool HasOpening { get; set; }
    string Tag { get; set; }
    bool HasSpatialStructure { get; set; }
    IIfcElement ThisElement { get; set; }
    void InitialElement(IIfcElement ifcBE);
    void AddRelatedProduct(IElementData ele);
    void SetDecomposeProduct(IEnumerable<IIfcRelDecomposes> relatedProd);
}
[Serializable]
public class ElementData : MonoBehaviour, IElementData
{
    private List<IProductData> relatedProducts = new List<IProductData>();
    private IProductData relatingProduct = null;
    [SerializeField]
    private MyBimProduct productGoe = MyBimProduct.Default;
    private IIfcElement thisElement;

    [SerializeField]
    private string typeName;
    [SerializeField]
    private string _name;
    [SerializeField]
    private int label;
    [SerializeField]
    private string globalID;
    [SerializeField]
    private string objectType;

    [SerializeField]
    private string elementTag;
    [SerializeField]
    private bool hasOpening;
    [SerializeField]
    private bool hasSpatialStructure = false;


    public GameObject ThisGameObject => gameObject;
    public MyBimProduct ProductGeoData { get => productGoe; set => productGoe = value; }
    public IIfcProduct ThisProduct { get => thisElement; set => thisElement = (IIfcBuildingElement)value; }
    public IProductData RelatingProduct { get => relatingProduct; set => relatingProduct = value; }
    public List<IProductData> RelatedProducts { get => relatedProducts; set => relatedProducts = value; }


    public string Name { get => _name; set => _name = value; }
    public string TypeName { get => typeName; set => typeName = value; }
    public int EntityLabel { get => label; set => label = value; }
    public string GlobalID { get => globalID; set => globalID = value;  }
    public string ObjectType { get => objectType; set => objectType = value; }


    public bool HasSpatialStructure { get => hasSpatialStructure; set => hasSpatialStructure = value; }
    public bool HasOpening { get => hasOpening; set => hasOpening = value; }
    public IIfcElement ThisElement { get => thisElement; set => thisElement = value; }
    public string Tag { get => elementTag; set => elementTag = value; }

    public void AddRelatedProduct(IElementData ele)
    {
        relatedProducts.Add(ele);
        ele.RelatingProduct = this;
        ele.ThisGameObject.transform.parent = ThisGameObject.transform;
    }

    public void InitialElement(IIfcElement ifcEle)
    {
        thisElement = ifcEle;

        elementTag = ifcEle.Tag;
        hasSpatialStructure = true;
        //int i = 0;
        //foreach(var o in thisElement.HasOpenings)
        //{
        //    i++;
        //}
        //if (i != 0) Debug.Log(ThisElement.Name);

        _name = thisElement.Name;
        typeName = thisElement.GetType().Name;
        label = thisElement.EntityLabel;
        globalID = thisElement.GlobalId;
        objectType = thisElement.ObjectType;

        

        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.Elements.Add(this);
    }

    public void SetDecomposeProduct(IEnumerable<IIfcRelDecomposes> connects)
    {
        foreach(var c in connects)
        {
            foreach(var prod in c.RelatedObjects)
            {
                var go = new GameObject();
                var ele = go.AddComponent<ElementData>();
                ele.InitialElement(prod as IIfcElement);
                AddRelatedProduct(ele);
            }
        }
    }
}
