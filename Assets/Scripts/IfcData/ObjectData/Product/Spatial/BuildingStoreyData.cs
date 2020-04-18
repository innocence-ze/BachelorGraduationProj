using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;
public interface IBuildingStoreyData : ISpatialData
{
    
}
public class BuildingStoreyData : SpatialData, IBuildingStoreyData
{
    //private List<IProductData> relatedProducts = new List<IProductData>();
    //private IProductData relatingProduct = null;
    //[SerializeField]
    //private MyBimProduct productGoe = MyBimProduct.Default;
    //private IIfcBuildingStorey thisBuildingStorey;

    //[SerializeField]
    //private string typeName;
    //[SerializeField]
    //private string _name;
    //[SerializeField]
    //private int label;
    //[SerializeField]
    //private string globalID;
    //[SerializeField]
    //private string objectType;

    //[SerializeField]
    //private string longName;

    //public string Name { get => _name; set => _name = value; }
    //public string TypeName { get => typeName; set => typeName = value; }
    //public int EntityLabel { get => label; set => label = value; }
    //public string GlobalID { get => globalID; set => globalID = value; }
    //public string ObjectType { get => objectType; set => objectType = value; }


    //public string LongName { get => longName; set => longName = value; }
    //public IIfcSpatialStructureElement ThisStructure { get => thisBuildingStorey; set => thisBuildingStorey = (IIfcBuildingStorey)value; }


    //public GameObject ThisGameObject { get => gameObject; }
    //public IIfcProduct ThisProduct { get => thisBuildingStorey; set => thisBuildingStorey = (IIfcBuildingStorey)value; }
    //public IProductData RelatingObject { get => relatingProduct; set => relatingProduct = value; }
    //public List<IProductData> RelatedObjects { get => relatedProducts; set => relatedProducts = value; }
    //public MyBimProduct ProductGeoData { get => productGoe; set => productGoe = value; }

    public void InitialSpatialElement(IIfcSpatialStructureElement ifcSE)
    {
        //thisBuildingStorey = (IIfcBuildingStorey)ifcSE;

        //longName = thisBuildingStorey.LongName;

        //_name = thisBuildingStorey.Name;
        //typeName = thisBuildingStorey.GetType().Name;
        //label = thisBuildingStorey.EntityLabel;
        //globalID = thisBuildingStorey.GlobalId;
        //objectType = thisBuildingStorey.ObjectType;

        ThisGameObject.name = Name + "[" + TypeName + "]#" + EntityLabel;
        SomeValue.spatialStructures.Add(this);
    }
    //public void AddRelatedProduct(IElementData ele)
    //{
    //    relatedProducts.Add(ele);
    //    ele.RelatingObject = this;
    //    if (ThisGameObject.transform.Find(ele.TypeName) == null)
    //        new GameObject(ele.TypeName).transform.parent = ThisGameObject.transform;
    //    ele.ThisGameObject.transform.parent = ThisGameObject.transform.Find(ele.TypeName);
    //}
    //public void AddRelatedProduct(ISpatialData spat)
    //{
    //    relatedProducts.Add(spat);
    //    spat.RelatingObject = this;
    //    spat.ThisGameObject.transform.parent = ThisGameObject.transform;
    //}
}

