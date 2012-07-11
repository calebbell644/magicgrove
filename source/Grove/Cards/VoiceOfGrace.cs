﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Mana;
  using Core.Dsl;

  public class VoiceOfGrace : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Voice of Grace")
        .ManaCost("{3}{W}")
        .Type("Creature Angel")
        .Text("{Flying}, protection from black")
        .FlavorText(
          "'Opposite Law is Grace, and Grace must be preserved. If the strands of Grace are unraveled, its design will be lost, and the people with it.'{EOL}—Song of All, canto 167")
        .Power(2)
        .Toughness(2)
        .Protections(ManaColors.Black)
        .Timing(Timings.Creatures())
        .Abilities(
          Static.Flying
        );
    }
  }
}