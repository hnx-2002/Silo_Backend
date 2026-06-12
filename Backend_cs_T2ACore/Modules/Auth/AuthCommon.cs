using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace T2ACore
{
    /// <summary>
    /// 鉴权方法
    /// </summary>
    public static class AuthCommon
    {
        /// <summary>
        /// 获取角色维度
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns></returns>
        public static (string Region, string RegionTitle) GetRegion(int roleType)
        {
            try
            {
                var regionTitle = Config.AuthConfig.RoleTitle[roleType];
                var region = "";
                switch (roleType)
                {
                    case 0:
                        region = "g";
                        break;
                    case 1:
                        region = "g2";
                        break;
                    case 2:
                        region = "g3";
                        break;
                    case 3:
                        region = "g4";
                        break;
                    case 4:
                        region = "g5";
                        break;
                    case 5:
                        region = "g6";
                        break;
                    case 6:
                        region = "g7";
                        break;
                    case 7:
                        region = "g8";
                        break;

                    default:
                        break;
                }
                return (region, regionTitle);
            }
            catch (Exception)
            {
                return ("", "");
            }
        }

        /// <summary>
        /// 获取资源类型
        /// </summary>
        /// <param name="assetsType"></param>
        /// <returns></returns>
        public static (string type, string asset) GetAssetsType(int assetsType)
        {
            try
            {
                var asset = Config.AuthConfig.Assets[assetsType];
                var type = "";
                switch (assetsType)
                {
                    case 0:
                        type = "p";
                        break;
                    case 1:
                        type = "p2";
                        break;
                    case 2:
                        type = "p3";
                        break;
                    case 3:
                        type = "p4";
                        break;
                    case 4:
                        type = "p5";
                        break;
                    case 5:
                        type = "p6";
                        break;
                    case 6:
                        type = "p7";
                        break;
                    case 7:
                        type = "p8";
                        break;
                    default:
                        break;
                }
                return (type, asset);
            }
            catch (Exception)
            {
                return ("", "");
            }
        }

        /// <summary>
        /// 通过反射获取所有的接口方法
        /// </summary>
        /// <returns></returns>
        public static List<ControllerFunction> GetControllerFunctions()
        {
            var res = new List<ControllerFunction>();
            var assem = AppDomain.CurrentDomain
                .GetAssemblies().ToList()
                .First(x => x.GetName().Name == "TPBackendDotnet");

            Type[] types = assem.GetTypes();

            // 遍历所有类型
            foreach (Type type in types)
            {
                if (type.Name.EndsWith("Controller"))
                {
                    var className = type.Name;
                    var temp = type.GetCustomAttributes(true);
                    var temp2 = temp.Cast<Attribute>();
                    var classAttr = temp2.First(x => x.GetType().Name == "RouteAttribute");

                    var routeAttr = classAttr as RouteAttribute;

                    // 获取类型中所有的方法
                    MethodInfo[] methods = type.GetMethods();

                    foreach (MethodInfo method in methods)
                    {
                        var func = new ControllerFunction();
                        func.GroupName = className;
                        func.FunctionName = method.Name;

                        var ttemp1 = method.GetCustomAttributes(true);
                        var ttemp2 = ttemp1.Cast<Attribute>();

                        Attribute ttemp3 = null;

                        foreach (var tt in ttemp2)
                        {
                            //FunConsole.ConsoleLog("attr", className, method.Name, tt.GetType().Name);

                            if (tt.GetType().Name.StartsWith("Http"))
                            {
                                ttemp3 = tt;
                            }
                        }

                        if (ttemp3 != null)
                        {
                            HttpMethodAttribute attr = ttemp3 as HttpMethodAttribute;

                            //   var attr = (HttpMethodAttribute)ttemp2.First(x => x.GetType().Name.StartsWith("Http"));

                            func.Method = attr.HttpMethods.FirstOrDefault();

                            func.Route = (routeAttr.Template.EndsWith("/")
                                          ? routeAttr.Template.Substring(0, routeAttr.Template.Length - 1)
                                          : routeAttr.Template) + "/" +
                                          (string.IsNullOrEmpty(attr.Template)
                                           ? ""
                                           : (attr.Template.StartsWith('/')
                                              ? attr.Template.Substring(1)
                                              : attr.Template));
                            res.Add(func);
                        }
                        else
                        {
                            foreach (var tt in ttemp2)
                            {
                                FunConsole.ConsoleLog("error", className, method.Name, tt.GetType().Name);

                            }
                        }

                    }


                }


            }

            return res;
        }

        /// <summary>
        /// 通过编号，获取属性
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static PropertyInfo GetAssetPatternName(int num)
        {
            Type assetType = typeof(Auth_assets_Class);
            return assetType.GetProperty("Value" + num.ToString());
        }
    }
}
