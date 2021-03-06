﻿namespace Grove.AI
{
  using System;
  using System.Diagnostics;

  public static class MatchSimulator
  {
    public static SimulationResult Simulate(Deck deck1, Deck deck2, int maxTurnsPerGame = 100,
      int maxSearchDepth = 16, int maxTargetsCount = 2)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var result = new SimulationResult();

      while (result.Deck1WinCount < 2 && result.Deck2WinCount < 2)
      {
        SimulateGame(deck1, deck2, result, maxTurnsPerGame, maxSearchDepth, maxTargetsCount);
      }

      stopwatch.Stop();

      result.Duration = stopwatch.Elapsed;

      return result;
    }

    private static void SimulateGame(Deck deck1, Deck deck2, SimulationResult result, int maxTurnsPerGame,
      int maxSearchDepth, int maxTargetsCount)
    {
      var stopwatch = new Stopwatch();

      var game = new Game(GameParameters.Simulation(deck1, deck2, 
        new SearchParameters(maxSearchDepth, maxTargetsCount, SearchPartitioningStrategies.SingleThreaded)));

      game.Ai.SearchStarted += delegate
        {
          result.TotalSearchCount++;
          stopwatch.Start();
        };

      game.Ai.SearchFinished += delegate
        {
          stopwatch.Stop();

          if (stopwatch.Elapsed > result.MaxSearchTime)
          {
            result.MaxSearchTime = stopwatch.Elapsed;
          }

          stopwatch.Reset();
        };

      game.Start(numOfTurns: maxTurnsPerGame);

      result.TotalTurnCount += game.Turn.TurnCount;

      if (game.Players.BothHaveLost)
        return;

      if (game.Players.Player1.Score > -game.Players.Player2.Score)
      {
        result.Deck1WinCount++;
        return;
      }

      result.Deck2WinCount++;
      return;
    }

    public class SimulationResult
    {
      public int Deck1WinCount { get; set; }
      public int Deck2WinCount { get; set; }
      public TimeSpan Duration { get; set; }
      public int TotalTurnCount { get; set; }
      public int TotalSearchCount { get; set; }
      public TimeSpan MaxSearchTime { get; set; }
    }
  }
}