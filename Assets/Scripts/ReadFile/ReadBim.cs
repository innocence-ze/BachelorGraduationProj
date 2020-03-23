using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

public class ReadBim
{
    /// <summary>
    /// return projectData of file, which contains lists of subSpatialStructure and subProduct  
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static IProjectData GetBimSpatialStructure(string fileName)
    {
        using (var model = IfcStore.Open(fileName))
        {
            var proj = model.Instances.FirstOrDefault<IIfcProject>();
            var go = new GameObject();
            var pd = go.AddComponent<ProjectData>();
            pd.Name = proj.Name;
            pd.ThisProject = proj;
            SomeValue.project = pd;
            go.name = pd.Name;

            foreach (var item in proj.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
                pd.AddSubSpatial(GetSpatialSturcture(proj, item));
            var spatialProducts = SomeValue.products.FindAll(p => p.HaveSpatialStructure == false);
            AddGeoProductToSpatialStructure(SomeValue.spatialStructures, spatialProducts);
            return pd;
        }


    }

    /// <summary>
    /// return spatial structure of cur IfcObject
    /// </summary>
    /// <param name="father"></param>
    /// <param name="cur"></param>
    /// <returns></returns>
    private static ISpatialData GetSpatialSturcture(IIfcObjectDefinition father,IIfcObjectDefinition cur)
    {
        ISpatialData sp = default;
        //whether cur is spatial structure or not
        var spatialElement = cur as IIfcSpatialStructureElement;
        if (spatialElement != null)
        {
            //get spatialElement's spatialData
            sp = InstantiateCurSpatial(spatialElement);
            if (sp != null)
            {
                SomeValue.spatialStructures.Add(sp);
                //get elements by using IfcRelContainedInSpatialElement 
                var containedElements = spatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
                if (containedElements.Count() > 0)
                {
                    foreach (var element in containedElements)
                    {
                        //use productData.entityLabel to find element's geomotry data
                        var prod = SomeValue.products.Find(p => p.ProductGeoData.entityLabel == element.EntityLabel);
                        //this is mainly because of some element is decomposed by some subElement.e.g.(stair=>stairFilght+Railing)
                        if (prod == null)
                        {
                            var go = new GameObject();
                            var pd = go.AddComponent<ProductData>();
                            pd.ProductGeoData = new MyBimProduct(element.EntityLabel, (short)element.EntityLabel);
                            SetProduct(pd, element);
                            sp.AddProduct(pd);
                            SetDecomposeProduct(pd,element.IsDecomposedBy);
                            //Debug.Log(sp.Name + " : " + element.Name + ", [" + element.GetType().Name + "] " + element.EntityLabel+" countain subProduct: " + element.IsDecomposedBy.Count());                           
                        }
                        else
                        {
                            SetProduct((ProductData)prod, element);
                            sp.AddProduct(prod);
                        }
                    }
                }
            }          
        }
        //use IfcRelAggregares to obtain sub spatial structure
        foreach (var item in cur.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
            sp.AddSubSpatial(GetSpatialSturcture(cur,item));
        return sp;
    }

    /// <summary>
    /// initialization parameters of productData:MonoBehaviour by ifcProduct
    /// </summary>
    /// <param name="pd"></param>
    /// <param name="p"></param>
    private static void SetProduct(ProductData pd, IIfcProduct p)
    {
        pd.ProductName = p.Name;
        pd.TypeName = p.GetType().Name;
        pd.ThisGameObject.name = pd.ProductName + "[" + pd.TypeName + "]#" + pd.ProductGeoData.entityLabel;
        pd.ThisProduct = p;
        pd.HaveSpatialStructure = true;
    }

    /// <summary>
    /// set decomposed product and return decomposingProducts
    /// </summary>
    /// <param name="productData"></param>
    /// <param name="connects"></param>
    /// <returns></returns>
    private static List<IProductData> SetDecomposeProduct(IProductData productData, IEnumerable<IIfcRelAggregates> connects)
    {
        List<IProductData> pds = new List<IProductData>();
        foreach(var c in connects)
        {
            foreach (var prod in c.RelatedObjects)
            {
                var pd = SomeValue.products.Find(p => p.ProductGeoData.entityLabel == prod.EntityLabel);
                SetProduct((ProductData)pd, (IIfcProduct)prod);
                pds.Add(pd);
            }
        }
        productData.DecomposedProducts = pds;
        return pds;
    }

    /// <summary>
    /// instantiate spatialData by ifcSpatialStructureElement
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static ISpatialData InstantiateCurSpatial(IIfcSpatialStructureElement s)
    {
        var go = new GameObject();
        ISpatialData sp;
        if(s is IIfcSite)
        {
            sp = go.AddComponent<SiteData>();
        }
        else if(s is IIfcBuilding)
        {
            sp = go.AddComponent<BuildingData>();
        }
        else if(s is IIfcBuildingStorey)
        {
            sp = go.AddComponent<BuildingStoreyData>();
        }
        else if(s is IIfcSpace)
        {
            sp = go.AddComponent<SpaceData>();
        }
        else
        {
            sp = null;
        }
        if (sp != null)
        {
            sp.Name = s.Name;
            sp.ThisStructure = s;
            go.name = sp.Name + "[" + sp.ThisStructure.GetType().Name + "]#" + sp.EntityLabel;
            SomeValue.spatialStructures.Add(sp);
        }
        return sp;
    }
   
    /// <summary>
    /// some spatialStructure have geomotry, append them
    /// </summary>
    /// <param name="sds"></param>
    /// <param name="pds"></param>
    private static void AddGeoProductToSpatialStructure(List<ISpatialData> sds, List<IProductData> pds)
    {
        foreach (var sd in sds)
        {
            var pd = pds.Find(p => p.ProductGeoData.entityLabel == sd.EntityLabel);
            if (pd != null)
            {
                pds.Remove(pd);
                SomeValue.products.Remove(pd);
                var spd = sd.ThisGameObject.AddComponent<ProductData>();
                spd.ProductGeoData = pd.ProductGeoData;
                spd.ThisProduct = pd.ThisProduct;

                var children = pd.ThisGameObject.GetComponentsInChildren<MeshRenderer>();
                if(sd.ThisStructure is IIfcSpace)
                    foreach (var c in children)
                    {
                        c.transform.parent = sd.ThisGameObject.transform;
                        c.gameObject.SetActive(false);
                    }
                else
                    foreach (var c in children)
                        c.transform.parent = sd.ThisGameObject.transform;
                Object.Destroy(pd.ThisGameObject);
            }
        }
    }

}
