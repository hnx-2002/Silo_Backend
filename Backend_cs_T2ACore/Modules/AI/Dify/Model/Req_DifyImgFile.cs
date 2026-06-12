using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2ACore;

/// <summary>
/// 
/// </summary>
public class Req_DifyImgFile
{
    /// <summary>
    /// 
    /// </summary>
    public string type { get; set; } = "image";

    /// <summary>
    /// 
    /// </summary>
    public string transfer_method { get; set; } = "local_file";

    /// <summary>
    /// 
    /// </summary>
    public string upload_file_id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Req_DifyImgFile New(string id)
    {
        var res = new Req_DifyImgFile();
        res.upload_file_id = id;
        return res;

    }
}
