using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface IWonderCardDecksInitialiser
    {
        WonderCardDecksModel InitialiseWonderCardDecksForBot(int playerCount);
    }
}
