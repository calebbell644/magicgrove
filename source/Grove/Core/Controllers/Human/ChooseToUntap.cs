﻿namespace Grove.Core.Controllers.Human
{
  using System.Windows;
  using Ui;
  using Ui.Shell;

  public class ChooseToUntap : Controllers.ChooseToUntap
  {
    public IShell Shell { get; set; }

    protected override void ExecuteQuery()
    {
      var result = Shell.ShowMessageBox(
        message: string.Format("Untap {0}?", Permanent),
        buttons: MessageBoxButton.YesNo,
        type: DialogType.Small);

      Result = result == MessageBoxResult.Yes;
    }
  }
}