﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Mana;
  using Core.Dsl;
  using Core.Targeting;

  public class Expunge : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Expunge")
        .ManaCost("{2}{B}")
        .Type("Instant")
        .Text(
          "Destroy target nonartifact, nonblack creature. It can't be regenerated.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Cycling("{2}")
        .Timing(Timings.TargetRemovalInstant())
        .Category(EffectCategories.Destruction)
        .Targets(
          filter: TargetFilters.Destroy(),
          selectors:
            C.Selector(Selectors.Creature((creature) => !creature.HasColor(ManaColors.Black) && !creature.Is().Artifact)))
        ;
    }
  }
}