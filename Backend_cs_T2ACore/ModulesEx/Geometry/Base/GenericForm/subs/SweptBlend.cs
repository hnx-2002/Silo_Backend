using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TPGeometryPro;

/// <summary>
/// 放样融合
/// 为方便序列化，反序列化操作，允许使用构造函数创建
/// 但仍强烈建议使用静态构造方法创建
/// </summary>
public class SweptBlend : GenericForm
{
    #region 构造函数，静态方法
    /// <summary>
    /// 使用曲线作为路径，将新的放样融合添加到族文档中。
    /// </summary>
    /// <param name="isSolid">指示放样融合是实心还是空心。</param>
    /// <param name="path">
    /// 放样融合的路径。路径应该是一条曲线。
    /// 或者路径可以是一条绘制的曲线，并且不需要该曲线来引用现有的几何图元。
    /// </param>
    /// <param name="pathPlane">
    ///【IMPORTANT!!!】原方法类型为SketchPlane,现改为Plane 
    /// 路径的草图平面。如果要创建驻留在现有平面上的二维路径，请使用此选项。
    /// 可选，对于从几何图形获得的路径曲线或路径不应参考现有边的二维路径，可以为null。
    /// </param>
    /// <param name="bottomProfile">
    ///【IMPORTANT!!!】原方法类型为SweepProfile,现改为List List Curve 
    /// 新创建的放样融合的底部轮廓。它应该只包含一个曲线循环。轮廓必须位于XY平面中，它将自动转换为轮廓平面。
    /// </param>
    /// <param name="topProfile">
    ///【IMPORTANT!!!】原方法类型为SweepProfile,现改为List List Curve 
    /// 新创建的放样融合的顶部轮廓。它应该只包含一个曲线循环。轮廓必须位于XY平面中，它将自动转换为轮廓平面。
    /// </param>
    /// <returns>如果创建成功，则返回新的放样融合，否则将引发包含失败信息的异常。</returns>
    /// <exception cref="ArgumentNullException">当输入参数path/bottomProfile/topProfile为null时抛出。</exception> 
    /// <exception cref="ArgumentException">当输入参数bottomProfile/topProfile是基于曲线的配置文件并且该配置文件为null时抛出。</exception>
    /// <exception cref="ArgumentException">当输入参数bottomProfile/topProfile是基于曲线的轮廓并且该轮廓包含null或多个曲线循环时抛出。</exception>
    /// <exception cref="ArgumentException">当输入参数bottomProfile/topProfile是基于族符号的配置文件并且族符号配置文件为null时抛出。</exception>
    /// <exception cref="InvalidOperationException">在概念体量、二维或其他无法创建放样融合的族中尝试创建时抛出。</exception>
    /// <exception cref="InvalidOperationException">创建失败时抛出。</exception>
    /// <remarks>
    /// 此方法在族文档中创建放样融合。放样融合将沿着路径从底部轮廓追踪到顶部。将为两个轮廓的顶点确定适当的默认映射。
    /// 如果输入轮廓是循环轮廓（曲线或椭圆），则必须将其至少拆分为两段，以便可以找到用于映射扫掠混合的顶点。
    /// </remarks>
    public static SweptBlend NewSweptBlend(bool isSolid, Curve path, Plane pathPlane,
        List<Curve> bottomProfile, List<Curve> topProfile)
    {
        if (path == null)
        {
            throw new ArgumentNullException("path不可为null");
        }

        if (pathPlane == null)
        {
            throw new ArgumentNullException("pathPlane不可为null");
        }

        if (bottomProfile == null)
        {
            throw new ArgumentNullException("bottomProfile不可为null");
        }

        if (topProfile == null)
        {
            throw new ArgumentNullException("topProfile不可为null");
        }

        if (bottomProfile.Any(x => x == null))
        {
            throw new ArgumentException("bottomProfile包含null曲线");
        }

        if (topProfile.Any(x => x == null))
        {
            throw new ArgumentException("topProfile包含null曲线");
        }


        var result = new SweptBlend();
        result.IsSolid = isSolid;
        result.Path = path;
        result.PathPlane = pathPlane;
        result.BottomProfile = bottomProfile;
        result.TopProfile = topProfile;

        return result;
    }

    #endregion 构造函数，静态方法

    #region 构造属性 

    /// <summary>
    /// 路径曲线 
    /// </summary>
    public Curve Path { get; set; }

    /// <summary>
    /// 路径所在平面
    /// </summary>
    public Plane PathPlane { get; set; }

    /// <summary>
    /// 底部多组轮廓的集合
    /// </summary>
    public List<Curve> BottomProfile { get; set; }

    /// <summary>
    /// 顶部多组轮廓的集合
    /// </summary>
    public List<Curve> TopProfile { get; set; }


    #endregion 构造属性

    #region 属性
    /// <summary>
    /// 放样融合的顶部轮廓草图所在的平面
    ///【IMPORTANT!!!】原方法类型为Sketch,现改为Plane  
    /// </summary>
    /// <remarks>
    ///【IMPORTANT!!!】忽略如下限制：如果顶部轮廓基于族符号，则此特性为null。 
    /// </remarks>
    public Plane TopSketch
    {
        get
        {
            //TODO 返回Top所在位置的面
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 放样融合的底部轮廓草图所在的平面
    ///【IMPORTANT!!!】原方法类型为Sketch,现改为Plane  
    /// </summary>
    /// <remarks>
    ///【IMPORTANT!!!】忽略如下限制：如果底部轮廓基于族符号，则此特性为null。 
    /// </remarks>
    public Plane BottomSketch
    {
        get
        {
            //TODO 返回Bottom所在位置的面
            throw new NotImplementedException();
        }
    }
    #endregion 属性

    #region 重写属性
    /// <summary>
    /// 实体类型
    /// </summary>
    public override GenericFormType GenericFormType
    {
        get
        {
            return GenericFormType.SweptBlend;
        }
    }
    #endregion 重写属性

    #region 需要进一步实现 属性
    //Public property AssemblyInstanceId The id of the assembly instance to which the element belongs.
    //(Inherited from Element.) 
    //Public property BoundingBox Retrieves a box that circumscribes all geometry of the element.
    //(Inherited from Element.)
    //Public property Category Retrieves a Category object that represents the category or sub category in which the element resides.
    //(Inherited from Element.)
    //Public property Combinations The geometry combinations that this element belongs to.
    //(Inherited from CombinableElement.) 
    //Public property CreatedPhaseId Id of a Phase at which the Element was created. 
    //(Inherited from Element.)
    //Public property DemolishedPhaseId Id of a Phase at which the Element was demolished.
    //(Inherited from Element.)
    //Public property DesignOption Returns the design option to which the element belongs.
    //(Inherited from Element.)
    //Public property Document Returns the Document in which the Element resides.
    //(Inherited from Element.)
    //Public property Geometry Retrieves the geometric representation of the element.
    //(Inherited from Element.)
    //Public property GroupId The id of the group to which an element belongs.
    //(Inherited from Element.)
    //Public property Id A unique identifier for an Element in an Autodesk Revit project.
    //(Inherited from Element.)
    //Public property IsSolid Identifies if the GenericForm is a solid or a void element.
    //(Inherited from GenericForm.)
    //Public property IsTransient Indicates whether an element is transient or permanent.
    //(Inherited from Element.)
    //Public property IsValidObject Specifies whether the.NET object represents a valid Revit entity. 
    //(Inherited from Element.)
    //Public property LevelId The id of the level associated with the element.
    //(Inherited from Element.)
    //Public property Location This property is used to find the physical location of an element within a project.
    //(Inherited from Element.) 
    //Public property OwnerViewId The id of the view that owns the element.
    //(Inherited from Element.)
    //Public property Parameter[([(Guid])])  Retrieves a parameter from the element given a GUID for a shared parameter.
    //(Inherited from Element.)
    //Public property Parameter[([(BuiltInParameter])])  Retrieves a parameter from the element given a parameter id.
    //(Inherited from Element.)
    //Public property Parameter[([(Definition])])  Retrieves a parameter from the element based on its definition.
    //(Inherited from Element.)
    //Public property Parameters Retrieves a set containing all of the parameters that are contained within the element.
    //(Inherited from Element.)
    //Public property ParametersMap Retrieves a map containing all of the parameters that are contained within the element.
    //(Inherited from Element.)
    //Public property PathSketch The sketched path for the swept blend.
    //Public property Pinned Identifies if the element has been pinned to prevent changes.
    //(Inherited from Element.)
    //Public property SelectedPath The selected curve used for the swept blend path.
    //Public property Subcategory The subcategory.
    //(Inherited from GenericForm.) 
    //Public property UniqueId A stable unique identifier for an element within the document.
    //(Inherited from Element.)
    //Public property ViewSpecific Identifies if the element is owned by a view.
    //(Inherited from Element.)
    //Public property Visible The visibility of the GenericForm.
    //(Inherited from GenericForm.)
    //Public property WorksetId Get Id of the Workset which owns the element.
    //(Inherited from Element.)
    #endregion 需要进一步实现 属性

    #region 需要进一步实现 方法

    ///// <summary>
    ///// 获取顶部和底部配置文件中顶点之间的映射
    ///// </summary>
    ///// <returns></returns> 
    ///// <remarks>
    ///// 每个顶点指的是从顶部轮廓和底部轮廓开始的曲线的起点。
    ///// </remarks>
    //public VertexIndexPairArray GetVertexConnectionMap()
    //{
    //}

    ///// <summary>
    ///// 设置顶部轮廓和底部轮廓中顶点之间的映射
    ///// </summary>
    ///// <param name="vertexMap"></param>
    ///// <exception cref="ArgumentNullException">当输入参数“vertex Map”为null时抛出</exception>
    ///// <exception cref="ArgumentException">如果输入参数“vertexMap”为空，则抛出。</exception>
    ///// <exception cref="InvalidOperationException">文档无法重新生成时抛出</exception>
    ///// 
    //public void SetVertexConnectionMap(VertexIndexPairArray vertexMap)
    //{
    //}

    //Public method ArePhasesModifiable Returns true if the properties CreatedPhaseId and DemolishedPhaseId can be modified for this Element.
    //(Inherited from Element.)
    //Public method CanBeHidden Indicates if the element can be hidden in the view.
    //(Inherited from Element.)
    //Public method CanBeLocked Identifies if the element can be locked.
    //(Inherited from Element.)
    //Public method CanDeleteSubelement Checks if given subelement can be removed from the element.
    //(Inherited from Element.)
    //Public method CanHaveAnalyticalModel Indicates whether the Element can have an Analytical Model.
    //(Inherited from Element.)
    //Public method CanHaveTypeAssigned()() () () Identifies if the element can have a type assigned.
    //(Inherited from Element.)
    //Public method ChangeTypeId(ElementId) Changes the type of the element.
    //(Inherited from Element.)
    //Public method DeleteEntity Deletes the existing entity created by %schema% in the element
    //(Inherited from Element.)
    //Public method DeleteSubelement Removes a subelement from the element.
    //(Inherited from Element.)
    //Public method DeleteSubelements Removes the subelements from the element.
    //(Inherited from Element.)
    //Public method Dispose(Inherited from Element.)
    //Public method Equals Determines whether the specified Object is equal to the current Object.
    //(Inherited from Object.)
    //Public method GetAnalyticalModel Retrieves writeable Analytical Model for Element.
    //(Inherited from Element.)
    //Public method GetAnalyticalModelId Retrieves the Element Id of the Analytical Model Element for this Element.
    //(Inherited from Element.)
    //Public method GetDependentElements Get all elements that, from a logical point of view, are the children of this Element.
    //(Inherited from Element.) 
    //Public method GetEntity Returns the existing entity corresponding to the Schema if it has been saved in the Element, or an invalid entity otherwise. 
    //(Inherited from Element.)
    //Public method GetEntitySchemaGuids Returns the Schema guids of any Entities stored in this element.
    //(Inherited from Element.)
    //Public method GetExternalFileReference Gets information pertaining to the external file referenced by the element.
    //(Inherited from Element.)
    //Public method GetExternalResourceReference Gets the ExternalResourceReference associated with a specified external resource type.
    //(Inherited from Element.)
    //Public method GetExternalResourceReferences Gets the full map of the external resource references referenced by the element.
    //(Inherited from Element.)
    //Public method GetGeneratingElementIds Returns the ids of the element(s) that generated the input geometry object. 
    //(Inherited from Element.)
    //Public method GetGeometryObjectFromReference Retrieve one geometric primitive contained in the element given a reference.
    //(Inherited from Element.)
    //Public method GetHashCode Serves as a hash function for a particular type.
    //(Inherited from Object.)
    //Public method GetMaterialArea Gets the area of the material with the given id.
    //(Inherited from Element.)
    //Public method GetMaterialIds Gets the element ids of all materials present in the element.
    //(Inherited from Element.)
    //Public method GetMaterialVolume Gets the volume of the material with the given id.
    //(Inherited from Element.)
    //Public method GetMonitoredLinkElementIds Provides the link instance IDs when the element is monitoring.
    //(Inherited from Element.)
    //Public method GetMonitoredLocalElementIds Provides the local element IDs when the element is monitoring.
    //(Inherited from Element.)
    //Public method GetOrderedParameters Gets the parameters associated to the element in order.
    //(Inherited from Element.)
    //Public method GetParameterFormatOptions Returns a FormatOptions override for the element Parameter, or a default FormatOptions if no override exists.
    //(Inherited from Element.)
    //Public method GetParameters Retrieves the parameters from the element via the given name.
    //(Inherited from Element.)
    //Public method GetPhaseStatus Gets the status of a given element in the input phase
    //(Inherited from Element.)
    //Public method GetSubelements Returns the collection of element subelements.
    //(Inherited from Element.)
    //Public method GetType Gets the Type of the current instance.
    //(Inherited from Object.)
    //Public method GetTypeId Returns the identifier of this element's type. 
    //(Inherited from Element.) 
    //Public method GetValidTypes() () () () Obtains a set of types that are valid for this element.
    //(Inherited from Element.)
    //Public method GetVertexConnectionMap Gets the mapping between the vertices in the top and bottom profiles.
    //Public method GetVisibility Gets the visibility for the generic form.
    //(Inherited from GenericForm.)
    //Public method HasPhases Returns true if this Element has the properties CreatedPhaseId and DemolishedPhaseId.
    //(Inherited from Element.)
    //Public method IsExternalFileReference Determines whether this Element represents an external file.
    //(Inherited from Element.) 
    //Public method IsHidden Identifies if the element has been permanently hidden in the view.
    //(Inherited from Element.)
    //Public method IsMonitoringLinkElement Indicate whether an element is monitoring any elements in any linked models.
    //(Inherited from Element.)
    //Public method IsMonitoringLocalElement Indicate whether an element is monitoring other local elements.
    //(Inherited from Element.)
    //Public method IsPhaseCreatedValid Returns true if createdPhaseId is an allowed value for the property CreatedPhaseId in this Element.
    //(Inherited from Element.)
    //Public method IsPhaseDemolishedValid Returns true if demolishedPhaseId is an allowed value for the property DemolishedPhaseId in this Element.
    //(Inherited from Element.)
    //Public method IsValidType(ElementId) Checks if given type is valid for this element.
    //(Inherited from Element.)
    //Public method LookupParameter Attempts to find a parameter on the element which has the given name.
    //(Inherited from Element.)
    //Public method RefersToExternalResourceReference Determines whether this Element uses external resources associated with a specified external resource type.
    //(Inherited from Element.) 
    //Public method RefersToExternalResourceReferences Determines whether this Element uses external resources. 
    //(Inherited from Element.)
    //Public method SetEntity Stores the entity in the element.If an Entity described by the same Schema already exists, it is overwritten.
    //(Inherited from Element.)
    //Public method SetVertexConnectionMap Sets the mapping between the vertices in the top and bottom profiles.
    //Public method SetVisibility Sets the visibility for the generic form.
    //(Inherited from GenericForm.)
    //Public method ToString Returns a string that represents the current object.
    //(Inherited from Object.)
    #endregion 需要进一步实现 方法
}
