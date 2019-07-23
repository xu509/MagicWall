using System.Collections.Generic;
/// <summary>
///     数据服务接口
/// </summary>
public interface IDaoService
{
    /// <summary>
    ///     获取企业
    /// </summary>
    /// <returns></returns>
    List<Enterprise> GetEnterprises();

    /// <summary>
    ///     获取活动
    /// </summary>
    /// <returns></returns>
    List<Activity> GetActivities();

    /// <summary>
    ///     获取活动
    /// </summary>
    /// <param name="act_id"></param>
    /// <returns></returns>
    Activity GetActivityDetail(int act_id);

    /// <summary>
    ///     获取活动详细信息
    /// </summary>
    /// <param name="act_id"></param>
    /// <returns></returns>
    List<ActivityDetail> GetActivityDetails(int act_id);

    /// <summary>
    ///     获取产品
    /// </summary>
    /// <returns></returns>
    List<Product> GetProducts();

    /// <summary>
    ///     获取产品详细
    /// </summary>
    /// <param name="pro_id"></param>
    /// <returns></returns>
    Product GetProductDetail(int pro_id);

    /// <summary>
    ///     获取产品具体详细
    /// </summary>
    /// <param name="pro_id"></param>
    /// <returns></returns>
    List<ProductDetail> GetProductDetails(int pro_id);

    /// <summary>
    ///     获取浮块数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    FlockData GetFlockData(DataType type);

    /// <summary>
    ///     搜索
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    List<SearchBean> Search(string keys);

    /// <summary>
    ///     获取定制图片
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    List<string> GetCustomImage(CustomImageType type);

    /// <summary>
    ///     获取显示配置
    /// </summary>
    /// <returns></returns>
    List<SceneConfig> GetShowConfigs();

    /// <summary>
    ///     获取配置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    AppConfig GetConfigByKey(string key);

    /// <summary>
    ///     获取视频详细
    /// </summary>
    /// <returns></returns>
    Video GetVideoDetail();
}
