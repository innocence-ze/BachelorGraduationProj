using System.Collections.Generic;
using UnityEngine;
using Xbim.Ifc2x3.Interfaces;

public interface IProductData 
{
    IIfcProduct ThisProduct { get; set; }
    IProductData RelatingProduct { get; set; }
    List<IProductData> RelatedProducts { get; set; }
    GameObject ThisGameObject { get; }
    MyBimProduct ProductGeoData { get; set; }

    string Name { get; set; }
    string TypeName { get; set; }
    int EntityLabel { get; set; }
    string GlobalID { get; set; }
    string ObjectType { get; set; }

}
