using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using EasyCrawling.Models;
using EasyCrawling.ViewModels;

namespace EasyCrawling.Helpers
{
    public class TreeHelper
    {
        public static TreeData GetTree(HtmlNode documentNode)
        {
            TreeData firstTreeNode = new TreeData();
            firstTreeNode.Text = "..."; 

            SetTreeUsingNextNode(firstTreeNode, documentNode);

            return firstTreeNode;           
        }
    
        public static void SetTreeUsingNextNode(TreeData parentNode, HtmlNode newDoc)
        {
            foreach (HtmlNode htmlNode in newDoc.ChildNodes)
            {
                /*Unnecessary tree data*/
                if (CrawlingHelper.IsText(htmlNode.Name) &&
                    htmlNode.InnerText.Trim() == "")
                    continue;

                /*Set parent text*/
                if (CrawlingHelper.IsText(htmlNode.Name) &&
                    htmlNode.InnerText.Trim() != "")
                {
                    parentNode.Text += " " + WebUtility.HtmlDecode(newDoc.InnerText);
                    //return;
                }                

                /*New tree data*/
                TreeData treeNode = new TreeData();
                if (htmlNode.Attributes.Count == 0)
                {
                    treeNode.Text = WebUtility.HtmlDecode(htmlNode.Name);
                    treeNode.Tag = new NodeTag(
                        htmlNode.Name,
                        htmlNode.Attributes.Count,
                        htmlNode.ChildNodes.Count);
                    //treeNode.Parent = parentNode;
                }
                else
                {
                    treeNode.Text = WebUtility.HtmlDecode(htmlNode.Name + CrawlingHelper.ConvertAttrToText(htmlNode));
                    treeNode.Tag = new NodeTag(
                        htmlNode.Name + CrawlingHelper.ConvertAttrToTag(htmlNode), 
                        htmlNode.Attributes.Count,
                        htmlNode.ChildNodes.Count);
                    //treeNode.Parent = parentNode;
                }

                /*Next Tree*/
                if (htmlNode.ChildNodes.Count == 0)
                {
                    treeNode.Text += WebUtility.HtmlDecode(" " + htmlNode.InnerText);
                    parentNode.Children.Add(treeNode);
                }
                else
                {
                    parentNode.Children.Add(treeNode);
                    SetTreeUsingNextNode(treeNode, htmlNode);
                }
            }
        }

        public static string GetAllPath(TreeDataViewModel node)
        {
            string xPath = "";
            while (node.Parent != null)
            {
                if (node.TreeData.Tag.ToString() != "")
                    xPath = "//" + node.TreeData.Tag + xPath;

                node = node.Parent;
            }

            return xPath;
        }

        public static string GetOnePath(TreeDataViewModel node)
        {
            string xPath = "";
            if (node.TreeData.Tag.ToString() != "")
                xPath = ".//" + node.TreeData.Tag;

            return xPath;
        }        

        public static void SetListUsingNode(
            List<TreeData> outList,          
            HtmlNode nowNode,
            string currentXPath)
        {
            TreeData newNode = new TreeData();

            if (nowNode.InnerText.Trim() != "")
            {
                newNode.Text = nowNode.InnerText;
                newNode.Tag = new NodeTag(
                    CrawlingHelper.GetAllXPath(nowNode),
                    nowNode.Attributes.Count,
                    nowNode.ChildNodes.Count,
                    nowNode.Name,
                    currentXPath);
                AddListIfNotExist(outList, newNode);
            }

            foreach (var attrNode in nowNode.Attributes)
            {
                newNode = new TreeData();
                newNode.Text = WebUtility.HtmlDecode(attrNode.Value);
                newNode.Tag = new NodeTag(
                    CrawlingHelper.GetAllXPath(nowNode),
                    nowNode.Attributes.Count,
                    nowNode.ChildNodes.Count,
                    attrNode.Name,
                    currentXPath);
                AddListIfNotExist(outList, newNode);
            }

            foreach (HtmlNode htmlNode in nowNode.ChildNodes)
            {
                SetListUsingNode(outList, htmlNode, currentXPath + CrawlingHelper.GetOneXPath(htmlNode));
            }
        }

        private static void AddListIfNotExist(List<TreeData> outList,TreeData newNode)
        {
            if (outList.FindIndex(valueItem => valueItem.Tag.Equals(newNode.Tag)) == -1)
            {
                outList.Add(newNode);
            }
        }
    }    
}