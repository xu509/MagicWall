using System.Collections.Generic;
/// <summary>
///     次级数据接口
/// </summary>
public interface IDaoSubService
{
    
    List<Enterprise> GetEnterprises(int themeId);

    List<Activity> GetActivities(int themeId);

    List<Product> GetProducts(int themeId);
}
