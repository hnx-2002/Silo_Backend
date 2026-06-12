using System;
using System.Collections.Generic;
using System.Text;

namespace TPGeometryPro;

/// <summary>
/// 放样
/// 为方便序列化，反序列化操作，允许使用构造函数创建
/// 但仍强烈建议使用静态构造方法创建
/// </summary>
public class Sweep : GenericForm
{
    #region 构造函数，静态方法
    /// <summary>
    /// 使用曲线元素的路径将新的放样形式添加到族文档中。
    /// </summary>
    /// <param name="isSolid">指示扫掠是实心扫掠还是空心扫掠。</param>
    /// <param name="path">
    ///【IMPORTANT!!!】原方法类型为CurveArray,现改为List Curve 
    /// 扫掠的路径。路径应该是2D，其中所有输入曲线都位于一个平面中，并且这些曲线不需要引用现有几何体
    /// </param>
    /// <param name="pathPlane">
    ///【IMPORTANT!!!】原方法类型为SketchPlane,现改为Plane 
    /// 路径的草图平面。如果要创建驻留在现有平面上的二维路径，请使用此选项。
    /// 可选，对于三维路径或路径不应引用现有面的二维路径，可以为null。
    /// </param>
    /// <param name="profile">
    /// 【IMPORTANT!!!】原方法类型为SweepProfile,现改为List Live Curve 
    /// 新创建的扫掠的轮廓。这可能包含多个曲线回路或轮廓族。
    /// 轮廓必须位于XY平面中，它将自动转换为轮廓平面。
    /// 每个循环必须是一个完全闭合的曲线循环，并且循环不得相交。
    /// 回路可以是未绑定的圆或椭圆，但其几何图形将一分为二，以满足拉伸中使用的草图的要求。
    /// </param>
    /// <param name="profileLocationCurveIndex">
    /// 路径曲线的索引。将在其上确定轮廓平面的曲线
    /// </param>
    /// <param name="profilePlaneLocation"> 
    /// 轮廓定位曲线上将确定轮廓平面的位置
    /// </param>
    /// <returns>如果创建成功，则返回新的Sweep，否则将引发包含失败信息的异常。</returns>
    /// <exception cref="ArgumentException">当输入参数路径为null或为空时抛出。</exception> 
    /// <exception cref="ArgumentNullException">当输入参数配置文件为null时抛出。</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 当输入参数profileLocationCurveIndex超出索引界限时抛出。
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 当输入参数profilePlaneLocation在profilePlaneLocation枚举中不存在时抛出。
    /// </exception>
    /// <exception cref="InvalidOperationException">在无法创建放样的概念体量、二维或其他族中尝试创建时抛出。</exception>
    /// <exception cref="InvalidOperationException">创建失败时抛出。</exception>
    /// <remarks>
    /// 此方法在族文档中创建扫掠。扫掠将沿着路径跟踪轮廓。
    /// </remarks>
    public static Sweep NewSweep(bool isSolid, List<Curve> path, Plane pathPlane,
        List<List<Curve>> profile, int profileLocationCurveIndex,
        ProfilePlaneLocation profilePlaneLocation)
    {

        if (path == null)
        {
            throw new ArgumentNullException("path不可为null");
        }

        if (pathPlane == null)
        {
            throw new ArgumentNullException("pathPlane不可为null");
        }

        if (profile == null)
        {
            throw new ArgumentNullException("profile不可为null");
        }

        if (profileLocationCurveIndex < 0)
        {
            throw new ArgumentOutOfRangeException("profileLocationCurveIndex索引不可小于0");
        }

        if (profileLocationCurveIndex > path.Count - 1)
        {
            throw new ArgumentOutOfRangeException("profileLocationCurveIndex索引越界");
        }

        var result = new Sweep();
        result.IsSolid = isSolid;
        result.Path = path;
        result.PathPlane = pathPlane;
        result.Profile = profile;
        result.ProfileLocationCurveIndex = profileLocationCurveIndex;
        result.ProfilePlaneLocation = profilePlaneLocation;

        return result;
    }
    #endregion 构造函数，静态方法

    #region 构造属性    
    /// <summary>
    /// 路径的曲线集合
    /// </summary>
    public List<Curve> Path { get; set; }
    /// <summary>
    /// 路径所在平面
    /// </summary>
    public Plane PathPlane { get; set; }
    /// <summary>
    /// 多组轮廓的集合
    /// </summary>
    public List<List<Curve>> Profile { get; set; }
    /// <summary>
    /// 轮廓线在哪个Curve上的索引
    /// </summary>
    public int ProfileLocationCurveIndex { get; set; }
    /// <summary>
    /// 轮廓平面所在位置
    /// </summary>
    public ProfilePlaneLocation ProfilePlaneLocation { get; set; }

    #endregion 构造属性

    #region 属性
    /// <summary>
    /// 扫掠的最大分段角度（以弧度为单位）。
    /// </summary>
    /// <remarks>
    /// 此特性用于检索/设置扫掠的最大线段角度。
    /// 仅当启用轨迹分割时才可设置。
    /// </remarks>
    public double MaxSegmentAngle { get; set; }

    /// <summary>
    /// 扫掠的轨迹分割选项
    /// </summary>
    /// <remarks>
    /// 此属性用于检索/设置扫描的轨迹分割状态。
    /// 如果return为true，则表示用户可以控制MaxSegmentAngle，否则为禁用。
    /// </remarks>
    public bool IsTrajectorySegmentationEnabled { get; set; }
    #endregion 属性

    #region 重写属性
    /// <summary>
    /// 实体类型
    /// </summary>
    public override GenericFormType GenericFormType
    {
        get
        {
            return GenericFormType.Sweep;
        }
    }
    #endregion 重写属性

    #region 需要进一步实现 方法 
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
    //Public method SetVisibility Sets the visibility for the generic form.
    //(Inherited from GenericForm.)
    //Public method ToString Returns a string that represents the current object.
    //(Inherited from Object.)
    #endregion 需要进一步实现 方法

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
    //Public property Pinned Identifies if the element has been pinned to prevent changes. 
    //(Inherited from Element.) 
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
}
