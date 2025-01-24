using Godot;

public partial class FeatureToggle : Node2D {
    [Export]
    private Godot.Collections.Array<FeatureToggleName> _activeFeatureToggles { get; set; }

    private static Godot.Collections.Array<FeatureToggleName> _toggledFeatures;

    private Control _ui;

    public override void _Ready() {
        _toggledFeatures = _activeFeatureToggles;
        foreach (FeatureToggleName featureToggle in _activeFeatureToggles) {
            DebugPrinter.Print("Activating FT: " + featureToggle, LogCategory.FeatureToggle);
        }
        CreateUI();
    }

    private void CreateUI() {
        _ui = new Control();
        VBoxContainer vbox = new VBoxContainer();
        
        foreach (FeatureToggleName featureToggle in _activeFeatureToggles) {
            Button button = new Button {
                Text = featureToggle.ToString(),
                ToggleMode = true,
                ButtonPressed = _toggledFeatures.Contains(featureToggle)
            };
            button.Toggled += (toggledOn) => OnToggleButtonPressed(featureToggle, toggledOn, button);
            vbox.AddChild(button);
        }

        _ui.AddChild(vbox);
        AddChild(_ui);
    }

    private void OnToggleButtonPressed(FeatureToggleName featureToggleName, bool toggledOn, Button button) {
        if (toggledOn) {
            if (!_toggledFeatures.Contains(featureToggleName)) {
                _toggledFeatures.Add(featureToggleName);
                DebugPrinter.Print("Activating FT: " + featureToggleName, LogCategory.FeatureToggle);
            }
        }
        else {
            if (_toggledFeatures.Contains(featureToggleName)) {
                _toggledFeatures.Remove(featureToggleName);
                DebugPrinter.Print("Deactivating FT: " + featureToggleName, LogCategory.FeatureToggle);
            }
        }
    }

    public static bool IsActive(FeatureToggleName featureToggleName) {
        bool active = _toggledFeatures.Contains(featureToggleName);
        if (active) {
            DebugPrinter.Print("FT Checked and Found Active: " + featureToggleName, LogCategory.FeatureToggle);
        }
        return active;
    }
}
