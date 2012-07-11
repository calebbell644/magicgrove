﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Mana;
  using Core.Dsl;

  public class DriftingMeadow : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Drifting Meadow")
        .Type("Land")
        .Text(
          "Drifting Meadow enters the battlefield tapped.{EOL}{T}: Add {W} to your mana pool.{EOL}{Cycling} {2}({2}, Discard this card: Draw a card.)")
        .Timing(Timings.Lands())
        .Cycling("{2}")
        .Abilities(
          C.ManaAbility(new ManaUnit(ManaColors.White), "{T}: Add {W} to your mana pool."))
        .Effect<PutIntoPlay>((e, _) => e.PutIntoPlayTapped = delegate { return true; });
    }
  }
}