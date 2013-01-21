﻿namespace Grove.Core.Preventions
{
  using Grove.Core.Targeting;

  public interface IDamagePreventionFactory
  {
    DamagePrevention Create(ITarget preventionOwner, Game game);
  }
}