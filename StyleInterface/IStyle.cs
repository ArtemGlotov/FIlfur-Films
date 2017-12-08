using System.Windows;

namespace StyleInterface
{
    public interface IStyle
    {
        string Name { get; }

        ResourceDictionary ChangeStyle();
    }
}
