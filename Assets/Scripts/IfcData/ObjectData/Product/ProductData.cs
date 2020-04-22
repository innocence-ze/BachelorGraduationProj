using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IProductData : IObjectData
{
    IIfcProduct ThisProduct { get; set; }
    MyBimProduct ProductGeoData { get; set; }

    string TypeName { get; set; }
    string ObjectType { get; set; }
}

public class ProductData : ObjectData,IProductData
{
    private IIfcProduct thisProduct;
    private MyBimProduct productGeoData = MyBimProduct.Default;
    private string typeName;
    private string objectType;

    public IIfcProduct ThisProduct { get => thisProduct; set => thisProduct = value; }
    public MyBimProduct ProductGeoData { get => productGeoData; set => productGeoData = value; }
    public string TypeName { get => typeName; set => typeName = value; }
    public string ObjectType { get => objectType; set => objectType = value; }

    
    public override void InitialObject(IIfcObject ifcObj)
    {
        base.InitialObject(ifcObj);
        thisProduct = ifcObj as IIfcProduct;
        typeName = thisProduct.GetType().Name;
        objectType = thisProduct.ObjectType;
        SetGeneralProperties();
    }

    public new void SetGeneralProperties()
    {
        generalProperties.Add("TypeName", typeName);
        generalProperties.Add("ObjectType", objectType);
    }
}