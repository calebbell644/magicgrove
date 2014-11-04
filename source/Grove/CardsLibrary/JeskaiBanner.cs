﻿namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using Costs;
  using Effects;

  public class JeskaiBanner : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Jeskai Banner")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{T}: Add {U}, {R}, or {W} to your mana pool.{EOL}{U}{R}{W}, {T}, Sacrifice Jeskai Banner: Draw a card.")
        .FlavorText("Discipline to persevere, insight to discover.")
        .ManaAbility(p =>
        {
          p.Text = "{T}: Add {U}, {R}, or {W} to your mana pool.";
          p.ManaAmount(Mana.Colored(isBlue: true, isRed: true, isWhite: true));
        })
        .ActivatedAbility(p =>
        {
          p.Text = "{U}{R}{W}, {T}, Sacrifice Jeskai Banner: Draw a card.";
          p.Cost = new AggregateCost(
            new PayMana("{U}{R}{W}".Parse(), ManaUsage.Abilities),
            new Tap(),
            new Sacrifice());
          p.Effect = () => new DrawCards(1);
        });
    }
  }
}