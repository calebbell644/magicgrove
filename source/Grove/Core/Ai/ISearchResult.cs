﻿namespace Grove.Core.Ai
{
  using System.Text;

  public interface ISearchResult
  {
    int? BestMove { get; }
    int? Score { get; }
    bool IsVisited { get; }
    void EvaluateSubtree();

    StringBuilder OutputBestPath(StringBuilder sb = null);
  }
}