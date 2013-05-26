﻿namespace Grove.UserInterface
{
  using Gameplay;
  using Gameplay.Misc;
  using Gameplay.Tournaments;
  using Messages;
  using Shell;

  public abstract class ViewModelBase : GameObject
  {
    public ViewModelFactories ViewModels { get; set; }
    public IShell Shell { get; set; }
    public new Game Game { get { return base.Game; } set { base.Game = value; } }
    public CardsInfo CardsInfo { get; set; }
    public Match Match { get; set; }
    public Tournament Tournament { get; set; }

    public void ChangePlayersInterest(Card card)
    {
      Shell.Publish(new PlayersInterestChanged
        {
          Visual = card
        });
    }

    public virtual void Initialize() {}
  }
}