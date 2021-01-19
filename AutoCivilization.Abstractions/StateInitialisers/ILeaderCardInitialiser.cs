using System.Threading.Tasks;

namespace AutoCivilization.Abstractions
{
    public interface ILeaderCardInitialiser
    {
        Task<LeaderCardModel> InitialiseRandomLeaderForBot();
    }
}
