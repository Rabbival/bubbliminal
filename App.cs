using Godot;
using System;

public partial class App : Node2D
{
	[Export]
	BubblesContainer _bubblesContainer;
	[Export]
	Label _gameWonLabel;

	public override void _Ready()
	{
		_gameWonLabel.Visible = false;
		_bubblesContainer.GameWon += (int score) => {
			_gameWonLabel.Text = "Game won!\nTurns taken: " + score;
			_gameWonLabel.Visible = true;
		};
	}
}
