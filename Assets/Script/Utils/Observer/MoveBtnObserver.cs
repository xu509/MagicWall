/// <summary>
/// 观察者模式 - 观察者
/// </summary>
namespace MagicWall
{
    public interface MoveBtnObserver
    {
        void Update();

        void goMove();

        void CancelMove();
    }
}