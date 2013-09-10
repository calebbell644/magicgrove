﻿namespace Grove.Gameplay.Zones
{
  using System;

  public class ZoneChangedEventArgs : EventArgs
  {
    public ZoneChangedEventArgs(Card card)
    {
      Card = card;      
    }

    public Card Card { get; private set; }    
  }
}