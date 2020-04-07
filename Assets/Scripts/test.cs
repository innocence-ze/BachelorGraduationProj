using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public UITree uiTree;
    private void Start()
    {
        UITreeData TreeRoot = new UITreeData("我的项目", new List<UITreeData>()
            {
                new UITreeData("unity专栏",new List<UITreeData>()
                {
                    new UITreeData("自定义UI组件专栏",new List<UITreeData>()
                    {
                        new UITreeData("目录树"),
                        new UITreeData("鼠标响应",new List<UITreeData>()
                        {
                            new UITreeData("OnMouseButton"),
                            new UITreeData("Ray")
                        })
                    }),
                    new UITreeData("调用外部exe"),
                    new UITreeData("Shader专栏",new List<UITreeData>()
                    {
                        new UITreeData("Dissolve Shader"),
                        new UITreeData("Transparent Shader")
                    })
                }),
                new UITreeData("ifc专栏",new List<UITreeData>()
                {
                    new UITreeData("基础篇"),
                    new UITreeData("高级篇",new List<UITreeData>()
                    {
                        new UITreeData("空间结构"),
                        new UITreeData("属性信息")
                    })
                })
            });
        uiTree.Inject(TreeRoot);
    }

}
