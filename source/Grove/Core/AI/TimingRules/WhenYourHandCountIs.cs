﻿namespace Grove.AI.TimingRules
{
  using System;
  using System.Linq;

  public class WhenYourHandCountIs : TimingRule
  {
    private readonly Func<Card, bool> _selector;
    private int? _maxCount;
    private int? _minCount;

    private WhenYourHandCountIs() {}

    public WhenYourHandCountIs(int? minCount = null, int? maxCount = null, Func<Card, bool> selector = null)
    {
      _selector = selector ?? delegate { return true; };
      _maxCount = maxCount;
      _minCount = minCount;
    }

    public override bool? ShouldPlay2(TimingRuleParameters p)
    {
      var result = true;

      if (_minCount.HasValue)
        result = result && p.Controller.Hand.Count(_selector) >= _minCount;

      if (_maxCount.HasValue)
        result = result && p.Controller.Hand.Count(_selector) <= _maxCount;

      return result;
    }
  }
}