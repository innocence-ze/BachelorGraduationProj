using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Xbim.Common.Geometry;
using Xbim.Common.XbimExtensions;
using Xbim.Ifc;
using Xbim.Ifc2x3.Interfaces;

public class BimReader
{
    #region GetSpatialHierarchy
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
            pd.InitialProject(proj);

            foreach (var item in proj.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
                pd.AddRelatedProduct(GetSpatialSturcture(item));

            //var spatialProducts = SomeValue.Elements.FindAll(p => p.HaveSpatialStructure == false);
            //AddGeoProductToSpatialStructure(SomeValue.spatialStructures, spatialProducts);

            return pd;
        }
    }

    /// <summary>
    /// return spatial structure of cur IfcObject
    /// </summary>
    /// <param name="father"></param>
    /// <param name="cur"></param>
    /// <returns></returns>
    private static ISpatialData GetSpatialSturcture(IIfcObjectDefinition cur)
    {
        ISpatialData spd = default;
        //whether cur is spatial structure or not
        if (cur is IIfcSpatialStructureElement spatialElement)
        {
            //get spatialElement's spatialData
            spd = InstantiateCurSpatial(spatialElement);
            if (spd != null)
            {
                //get elements by using IfcRelContainedInSpatialElement 
                var containedElements = spatialElement.ContainsElements.SelectMany(rel => rel.RelatedElements);
                if (containedElements.Count() > 0)
                {
                    foreach (var element in containedElements)
                    {
                        //use productData.entityLabel to find element's geomotry data
                        /*var ele = SomeValue.buildingElements.Find(e => e.ProductGeoData.entityLabel == element.EntityLabel);
                        if(!(element is IIfcElement))
                        {
                            continue;
                        }
                        //this is mainly because of some element is decomposed by some subElement.e.g.(stair=>stairFilght+Railing)
                        if (ele == null)
                        {
                            var go = new GameObject();
                            var pd = go.AddComponent<ElementData>();
                            pd.ProductGeoData = new MyBimProduct(element.EntityLabel, (short)element.EntityLabel);
                            pd.InitialElement(element as IIfcElement);
                            sp.AddRelatedProduct(pd);
                            SetDecomposeProduct(pd,element.IsDecomposedBy);
                            //Debug.Log(sp.Name + " : " + element.Name + ", [" + element.GetType().Name + "] " + element.EntityLabel+" countain subProduct: " + element.IsDecomposedBy.Count());                           
                        }
                        else
                        {
                            ele.InitialElement(element as IIfcElement);
                            sp.AddRelatedProduct(ele);
                        }*/
                        if (element is IIfcElement)
                        {
                            var go = new GameObject();
                            var pd = go.AddComponent<ElementData>();
                            pd.InitialElement(element as IIfcElement);
                            spd.AddRelatedProduct(pd);
                            if (element.IsDecomposedBy != null && element.IsDecomposedBy.Count() > 0)
                            {
                                pd.SetDecomposeProduct(element.IsDecomposedBy);
                            }
                        }
                    }
                }
            }
        }

        //use IfcRelAggregares to obtain sub spatial structure
        foreach (var item in cur.IsDecomposedBy.SelectMany(r => r.RelatedObjects))
            spd.AddRelatedProduct(GetSpatialSturcture(item));
        return spd;
    }

    /// <summary>
    /// set decomposed product and return decomposingProducts
    /// </summary>
    /// <param name="eleData"></param>
    /// <param name="connects"></param>
    /// <returns></returns>
    //private static List<IElementData> SetDecomposeProduct(IElementData eleData, IEnumerable<IIfcRelDecomposes> connects)
    //{
    //    List<IElementData> eds = new List<IElementData>();
    //    foreach(var c in connects)
    //    {
    //        foreach (var prod in c.RelatedObjects)
    //        {
    //            var ed = SomeValue.Elements.Find(p => p.ProductGeoData.entityLabel == prod.EntityLabel);
    //            ed.InitialElement(prod as IIfcElement);
    //            eds.Add(ed);
    //            eleData.AddRelatedProduct(ed);
    //        }
    //    }
    //    return eds;
    //}

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
            sp.InitialSpatialElement(s);
        }
        return sp;
    }
   
    /// <summary>
    /// some spatialStructure have geomotry, append them
    /// </summary>
    /// <param name="sds"></param>
    /// <param name="pds"></param>
   /* private static void AddGeoProductToSpatialStructure(List<ISpatialData> sds, List<IElementData> pds)
    {
        foreach (var sd in sds)
        {
            var pd = pds.Find(p => p.ProductGeoData.entityLabel == sd.EntityLabel);
            if (pd != null)
            {
                pds.Remove(pd);
                SomeValue.Elements.Remove(pd);
                var spd = sd.ThisGameObject.AddComponent<ElementData>();
                spd.ProductGeoData = pd.ProductGeoData;
                spd.ThisElement = pd.ThisElement;

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
    }*/
    #endregion

    /// <summary>
    /// read geomotry data from wexbim file
    /// </summary>
    /// <param name="fileName"></param>
    public static void ReadWexbimFile(string fileName)
    {
        Vector3 offsite;
        using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            using (var br = new BinaryReader(fs))
            {
                var magicalNum = br.ReadInt32();
                var version = br.ReadByte();
                var shapeCount = br.ReadInt32();
                var vertexCount = br.ReadInt32();
                var triangleCount = br.ReadInt32();
                var matrixCount = br.ReadInt32();
                var productCount = br.ReadInt32();
                var styleCount = br.ReadInt32();
                var scale = br.ReadSingle();
                var regionCount = br.ReadInt16();

                //Region
                for (int i = 0; i < regionCount; i++)
                {
                    var population = br.ReadInt32();
                    var centreX = br.ReadSingle(); centreX /= scale;
                    var centreY = br.ReadSingle(); centreY /= scale;
                    var centreZ = br.ReadSingle(); centreZ /= scale;
                    var boundsBytes = br.ReadBytes(6 * sizeof(float));
                    var bounds = XbimRect3D.FromArray(boundsBytes);
                    bounds.X /= scale; bounds.Y /= scale; bounds.Z /= scale;
                    bounds.SizeX /= scale; bounds.SizeY /= scale; bounds.SizeZ /= scale;
                    MyBimGeomorty.regions.Add(new MyBimRegion(population, centreX, centreY, centreZ, (float)bounds.SizeX, (float)bounds.SizeY, (float)bounds.SizeZ));
                }
                offsite = MyBimGeomorty.regions[0].position;

                //texture
                for (int i = 0; i < styleCount; i++)
                {
                    var styleId = br.ReadInt32();
                    var red = br.ReadSingle();
                    var green = br.ReadSingle();
                    var blue = br.ReadSingle();
                    var alpha = br.ReadSingle();
                    MyBimGeomorty.colors.Add(new MyBimColor(styleId, red, green, blue, alpha));
                }

                //product
                for (int i = 0; i < productCount; i++)
                {
                    var entityLabel = br.ReadInt32();
                    var typeId = br.ReadInt16();
                    var boxBytes = br.ReadBytes(6 * sizeof(float));
                    XbimRect3D bb = XbimRect3D.FromArray(boxBytes);
                    //float x = (float)bb.X, y = (float)bb.Y, z = (float)bb.Z;
                    //float sizeX = (float)bb.SizeX, sizeY = (float)bb.SizeY, sizeZ = (float)bb.SizeZ;
                    MyBimGeomorty.products.Add(new MyBimProduct(entityLabel, typeId));
                }

                //shape
                for (int i = 0; i < shapeCount; i++)
                {
                    var shapeRepetition = br.ReadInt32();
                    MyBimShapeInstance si;
                    if (shapeRepetition > 1)
                    {
                        List<MyBimShapeInstance> curShapeInstances = new List<MyBimShapeInstance>();
                        for (int j = 0; j < shapeRepetition; j++)
                        {
                            var ifcProductLabel = br.ReadInt32();
                            var ifcTypeId = br.ReadInt16();
                            var instanceLabel = br.ReadInt32();
                            var styleLabel = br.ReadInt32();
                            var transform = XbimMatrix3D.FromArray(br.ReadBytes(sizeof(double) * 16));

                            si = new MyBimShapeInstance(ifcProductLabel, ifcTypeId, instanceLabel, styleLabel, transform);
                            //MyBimGeomorty.shapeInstances.Add(si);
                            curShapeInstances.Add(si);
                            var p = MyBimGeomorty.products.Find(product => product.entityLabel == ifcProductLabel);
                            p.AddShapeInstance(si);
                        }
                        var triangulation = br.ReadShapeTriangulation();

                        foreach (var csi in curShapeInstances)
                        {
                            var tri = new MyBimTriangulation(triangulation, offsite, scale, csi.transform, true);
                            csi.AddTriangulation(tri);
                            //MyBimGeomorty.triangulations.Add(tri);
                        }
                    }
                    else if (shapeRepetition == 1)
                    {
                        var ifcProductLabel = br.ReadInt32();
                        var ifcTypeId = br.ReadInt16();
                        var instanceLabel = br.ReadInt32();
                        var styleLabel = br.ReadInt32();

                        si = new MyBimShapeInstance(ifcProductLabel, ifcTypeId, instanceLabel, styleLabel);
                        //MyBimGeomorty.shapeInstances.Add(si);
                        var p = MyBimGeomorty.products.Find(product => product.entityLabel == ifcProductLabel);
                        p.AddShapeInstance(si);

                        XbimShapeTriangulation triangulation = br.ReadShapeTriangulation();
                        var tri = new MyBimTriangulation(triangulation, offsite, scale);
                        //MyBimGeomorty.triangulations.Add(tri);

                        si.AddTriangulation(tri);
                    }
                }
            }
        }
    }
}
