using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class ProjectController : MonoBehaviour
{

    public UITree uiTree;

    private string ifcFileName;
    private string wexbimFileName;

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 90, 40), "Open"))
        {
            var fullFileName = OpenDialogFile();
            if (fullFileName != null)
            {
                SomeValue.fileName = fullFileName.Replace(".ifc", "");
                wexbimFileName = SomeValue.WexbimFileName;
                ifcFileName = SomeValue.BimFileName;
                GenerateWexBim(ifcFileName);
                RenderIFCModel();
                GenerateUiTree(SomeValue.project);
            }
        }
    }

    private void RenderIFCModel()
    {

        /*
        //generate Model
        BimReader.ReadWexbimFile(SomeValue.WexbimFileName);
        foreach(var p in MyBimGeomorty.products)
        {
            GenerateModel.GenerateElement(p);
        }

        //generate spatial structure
        var projData = BimReader.GetBimSpatialStructure(SomeValue.BimFileName);
        GenerateModel.GenerateRelatedProducts(projData);
        foreach(var p in SomeValue.Elements)
        {
            GenerateModel.AppendCollider(p);
        }*/
        var projData = BimReader.GetBimSpatialStructure(ifcFileName);
        BimReader.ReadWexbimFile(wexbimFileName);

        var allPD = new List<IProductData>();
        allPD.AddRange(SomeValue.Elements);
        allPD.AddRange(SomeValue.spatialStructures);

        foreach (var p in MyBimGeomorty.products)
        {
            var pd = GenerateModel.GenerateProduct(p, allPD);
            if (pd == null)
                print(p.entityLabel);
            GenerateModel.AppendCollider(pd);
        }
        projData.ThisGameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    private string OpenDialogFile()
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "IFC文件(*.ifc)\0*.ifc";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//默认路径
        openFileName.title = "选择所需的ifc文件";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (OpenFileName.GetSaveFileName(openFileName))
        {
            return openFileName.file;
        }
        else
        {
            return null;
        }
    }

    private void GenerateWexBim(string ifcFileName)
    {
        using(Process myProcess = new Process())
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(SomeValue.processName, ifcFileName);
            myProcess.StartInfo = startInfo;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.Start();
            myProcess.WaitForExit();
        }
    }

    private void GenerateUiTree(IProjectData projData)
    {
        var uiTreeData = new UITreeData(projData.Name, projData.EntityLabel);
        foreach(var prodData in projData.RelatedProducts)
        {
            Helper(prodData, uiTreeData);
        }
        uiTree.Inject(uiTreeData);
    }

    private void Helper(IProductData prodData, UITreeData parUiTree)
    {
        var curUiTree = new UITreeData(prodData.Name, prodData.EntityLabel);
        if(prodData is IElementData)
        {
            var typeUiTreeData = parUiTree.FindChildren(prodData.ProductGeoData.typeId);
            if (typeUiTreeData == default)
            {
                typeUiTreeData = new UITreeData(prodData.TypeName, prodData.ProductGeoData.typeId);
                parUiTree.AddChild(typeUiTreeData);
            }
            var curEleUiTreeData = new UITreeData(prodData.Name, prodData.EntityLabel);
            typeUiTreeData.AddChild(curEleUiTreeData);
            foreach(var decEle in prodData.RelatedProducts)
            {
                curEleUiTreeData.AddChild(new UITreeData(decEle.Name, decEle.EntityLabel));
            }
            return;
        }
        parUiTree.AddChild(curUiTree);
        foreach(var p in prodData.RelatedProducts)
        {
            Helper(p, curUiTree);
        }
    }
}
 