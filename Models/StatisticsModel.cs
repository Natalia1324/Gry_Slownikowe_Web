namespace Gry_Slownikowe.Models
{
    public class StatisticsModel
    {
        public string Nick { get; set; }
        public List<GameStatistics> GameStatistics { get; set; }

    }
    public class GameStatistics
    {
        public string GameName { get; set; }
        public int TotalGames { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
