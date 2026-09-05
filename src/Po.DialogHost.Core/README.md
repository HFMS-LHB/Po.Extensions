# Po.DialogHost.Core

Control-agnostic dialog framework core for Avalonia MVVM applications.

This package provides the ViewModel base classes (`PoDialogBase`, `FormDialogBase`, `TaskDialogBase`),
the `IPoDialogService` contract, and the `IDialogHostAdapter` abstraction.

To actually show dialogs, pair it with an adapter package:

- `Po.DialogHost.DialogHostAvalonia` — uses DialogHost.Avalonia.
- `Po.DialogHost.Ursa` — uses Ursa OverlayDialog.
