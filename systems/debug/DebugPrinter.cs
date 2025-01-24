using Godot;
using System;

public partial class DebugPrinter : Node
{
	[Export]
	private Godot.Collections.Array<LogCategory> _categoriesToPrint { get; set; }

	private static Godot.Collections.Array<LogCategory> _loggedCategories;

	public override void _Ready() {
		_loggedCategories = _categoriesToPrint;
	}

	public static ActionPerformed Print(String message, params LogCategory[] categories) {
		bool printed = false;
		foreach (LogCategory category in categories) {
			if (_loggedCategories.Contains(category)) {
				GD.Print(message);
				printed = true;
				break;
			}
		}
		return new ActionPerformed(printed);
	}
}

