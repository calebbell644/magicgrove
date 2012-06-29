﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.CardDsl;

  public class Acridian : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Acridian")
        .ManaCost("{1}{G}")
        .Type("Creature Insect")
        .Text(
          "{Echo} {1}{G} (At the beginning of your upkeep, if this came under your control since the beginning of your last upkeep, sacrifice it unless you pay its echo cost.)")
        .FlavorText(
          "The elves of Argoth were trained to ride these creatures, even when their mounts traveled upside-down.")
        .Power(2)
        .Toughness(4)
        .Echo("{1}{G}");
    }
  }
}