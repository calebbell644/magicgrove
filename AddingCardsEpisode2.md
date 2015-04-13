# Adding cards Episode 2 #

Welcome back! In this 2nd tutorial we will investigate:

  * the difference between spells and permanents,
  * basic mechanics of targeting,
  * activated abilities and
  * mana abilities.

We will look at the implementations of the following cards:

  * Volcanic Hammer,
  * Dragon Blood,
  * Birds of Paradise,

## An example of a sorcery spell: 'Volcanic Hammer' ##

Volcanic Hammer is a sorcery spell with casting cost of {1}{R}. When we cast the spell, we must first pick a target (creature or player). When resolved, the spell deals 3 damage to selected target and is put into graveyard. The following code shows the implementation of the card:

```

public class VolcanicHammer : CardTemplateSource
{
  public override IEnumerable<CardTemplate> GetCards()
  {
     yield return Card
        .Named("Volcanic Hammer")
        .ManaCost("{1}{R}")
        .Type("Sorcery")
        .Text("Volcanic Hammer deals 3 damage to target creature or player.")
        .FlavorText("Fire finds its form in the heat of the forge.")
        .Cast(p =>
          {
            p.Effect = () => new DealDamageToTargets(3);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectDealDamage(3));
          });
  }
}

```

A Casting rule is associated with every card's type. Casting rules define whether a card can be cast at a given time and what happens after it resolves. Let's look at the casting rule for sorceries:

```

public class Sorcery : CastingRule
{
  private readonly Action<Card> _afterResolvePutToZone;

  private Sorcery() {}

  public Sorcery(Action<Card> afterResolvePutToZone = null)
  {
    _afterResolvePutToZone = afterResolvePutToZone ?? (card => card.PutToGraveyard());
  }

  public override bool CanCast()
  {
    return Turn.Step.IsMain() &&
      Card.Controller.IsActive &&
        Stack.IsEmpty;
  }

  public override void AfterResolve()
  {
    _afterResolvePutToZone(Card);
  }
}

```

We see that sorceries can only be cast during the main phases of the active turn and only when the stack is empty.
By inspecting the AfterResolve method, we can see that the sorceries spells are put into graveyard by default. This can be overridden by afterResolvePutToZone parameter of the constructor (this is used by cards like 'Beacon of Destruction', which are shuffled into library instead of put into graveyard).

What happens when a card is cast? Let's look at the Cast method of our hammer once again.

```
.Cast(p =>
  {
    p.Effect = () => new DealDamageToTargets(3);
    p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
    p.TargetingRule(new EffectDealDamage(3));
  });
```

We didn't have to implement this method in our previous tutorial because the default implementation for creatures was automatically used (the creature is put into play by default).

Cast method accepts a delegate, which should initialize the CastInstructionParameters object passed into it as parameter p.

When cast, most cards create an effect object and put it on top of the stack. When priority is passed by both players, the top effect on the stack is resolved (and its action is applied to the game). We define an EffectFactory to create DealDamageToTargets effect:

```
p.Effect = () => new DealDamageToTargets(3);
```

Each time the factory will be called, it will return a new effect of type DealDamageToTargets.
The hammer can target a creature on the battlefield or a player. This is given by the following line of code:

```
p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
```

If AI would consider every legal target, this would waste a lot of time simulating moves a real player would never make. For example, one would very rarely want to target oneself or their own creatures with 'Volcanic Hammer'. To incorporate this knowledge into the game, the targeting rules are used. These rules reduce the amount of targets AI has to consider, enabling deeper search trees. We specify which targeting rule will be used by calling TargetingRule method:

```
p.TargetingRule(new EffectDealDamage(3));
```

## Implementing activated ability: 'Dragon Blood' ##

Dragon Blood is an artifact which, when activated, puts a +1/+1 counter on target creature. Its implementation is:

```

public class DragonBlood : CardTemplateSource
{
  public override IEnumerable<CardTemplate> GetCards()
  {
     yield return Card
        .Named("Dragon Blood")
        .ManaCost("{3}")
        .Type("Artifact")
        .Text("{3},{T} : Put a +1/+1 counter on target creature.")
        .FlavorText("Fire in the blood, fire in the belly.")
        .Cast(p => p.TimingRule(new OnFirstMain()))
        .ActivatedAbility(p =>
          {
            p.Text = "{3},{T} : Put a +1/+1 counter on target creature.";
            p.Cost = new AggregateCost(
              new PayMana(3.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new ApplyModifiersToTargets(() => new AddCounters(
              () => new PowerToughness(1, 1), count: 1));

            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
            p.TargetingRule(new EffectPump(1, 1, untilEot: false));
          });
  }
}

```

Game rules allow that permanents are cast either during First or Second main phase. Inside the Cast method we define a restriction that AI is only allowed to cast Dragon Blood during the First main phase (Dragon Blood is a combat spell and playing it before combat is generally a good idea). This is a performance optimization; if it were omitted, the number of states the AI would need to search would increase, although the card would still work.

Activated abilities are defined by assigning the members of the ActivatedAbilityParameters class, which is passed into ActivatedAbility method as parameter p. All activated abilities must define at least the following members:

  * Text (displayed in ui),
  * Cost (resources needed for activation),
  * Effect (the effect ability has on the game)

In order to activate Dragon Blood, we need to pay 3 colorless mana and tap it. If a cost needs more than one resource, its parts are wrapped with AggregateCost object. To support mana sources with restricted usage, we must also specify intended mana usage.

By adding a +1/+1 counter on target creature, we modify the creature's characteristics (the number of counters). We use ApplyModifiersToTargets effect to apply one or more modifiers to the effect's targets.

The line:

```
() => new AddCounters(() => new PowerToughness(1, 1), count: 1)
```

defines a modifier factory which produces AddCounters modifier when the effect is resolved. When the modifier is applied to target creature, it creates +1/+1 counter and puts it on the creature.

All modifiers in magicgrove have a limited lifetime. The default lifetime for permanent modifiers is as long as the owning permanent stays on the battlefield. In our case, this is exactly the needed behavior.

Since the activated ability's effect requires targets, we must also specify the target validation logic and an appropriate AI targeting rule:

```
p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());
p.TargetingRule(new EffectPump(1, 1, untilEot: false));
```

EffectPump targeting rule limits the range of targets considered by the AI. As AI considers different targets at different steps of the game, the AI timing rule is already part of the targeting rule, so in this case no explicit timing rules are needed.

## Implementing mana abilities: 'Birds of paradise' ##

AI does not 'activate' mana abilities in the same way as activated abilities (although a human player can by tapping the lands manually). The reason is, this would considerably increase the number of moves it has to check  and many of them would be useless, e.g tapping three lands for mana is useless when a spell costs four.

Mana abilities are therefore only activated when certain mana is needed by other abilities or spells. Let's look an example:

```
public class BirdsOfParadise : CardTemplateSource
{
  public override IEnumerable<CardTemplate> GetCards()
  {
    yield return Card
      .Named("Birds of Paradise")
      .ManaCost("{G}")
      .Type("Creature - Bird")
      .Text("{Flying}{EOL}{T}: Add one mana of any color to your mana pool.")
      .FlavorText("The gods used their feathers to paint all the colors of the world.")
      .Power(0)
      .Toughness(1)
      .SimpleAbilities(Static.Flying)
      .ManaAbility(p =>
        {
          p.Text = "{T}: Add one mana of any color to your mana pool.";
          p.ManaAmount(Mana.Any);
        });
  }
}
```

Birds of Paradise is a creature with flying, capable of producing one mana of any color when tapped. Bird's mana ability is defined by calling ManaAbility method on CardTemplate class and assigning members of the ManaAbilityParameters object:

```
.ManaAbility(p =>
  {
    p.Text = "{T}: Add one mana of any color to your mana pool.";
    p.ManaAmount(Mana.Any);
  })
```

The most common way of paying the activation cost of mana abilities is to tap the owning card. Since this is so common, we decided that it would be the default cost and does not need to be explicitly specified.
The amount of mana the ability will produce is defined using ManaAmount method.

That's all for now, have fun and see you soon.