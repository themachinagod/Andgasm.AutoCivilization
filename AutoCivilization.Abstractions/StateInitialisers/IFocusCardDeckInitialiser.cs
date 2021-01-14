using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface IFocusCardDeckInitialiser
    {
        Task InitialiseFocusCardsDeckForBot();
    }
}
