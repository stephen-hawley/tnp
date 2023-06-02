using System;
using Microsoft.Maui.Controls.Shapes;
using tnp.ViewModels;
using tnp.Views;

namespace tnp.Models
{
	public class HelloWorldNodeView : ContentView, INodeView
	{
		public Node LinkedNode { get; set; }

		public HelloWorldNodeView(Node node)
		{
			LinkedNode = node;

			var border = new Border();

			var grid = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition() { Height = new GridLength(40, GridUnitType.Absolute) },
					new RowDefinition() { Height = new GridLength(9, GridUnitType.Star) },
				},
				ColumnDefinitions =
				{
					new ColumnDefinition() {Width = new GridLength(9, GridUnitType.Star)},
					new ColumnDefinition() {Width = new GridLength(40, GridUnitType.Absolute)},
				},
			};


			var nameEntry = new Entry();
			nameEntry.SetBinding(Entry.TextProperty, "Name");
			nameEntry.BindingContext = node;
			nameEntry.Text = "HelloWorldNode";
			nameEntry.TextColor = Colors.White;
			nameEntry.FontSize = 30;


			var printLabel = new Label();
			printLabel.Text = "Print: ";
			printLabel.TextColor = Colors.White;
			printLabel.FontSize = 30;


			var printEntry = new Entry();
			printEntry.SetBinding(Entry.TextProperty, "PrintString");
			printEntry.BindingContext = node;
			printEntry.Text = "Hello World!";
			printEntry.TextColor = Colors.White;
			printEntry.FontSize = 30;

			var layout = new StackLayout();
			layout.Spacing = 10;
			layout.Children.Add(nameEntry);
			layout.Children.Add(new HorizontalStackLayout
			{
				printLabel,
				printEntry,
			});

			border.Background = Colors.LightBlue;



			var cancelButton = new ImageButton
			{
				//Background = Colors.Red,
				Margin = 5,
				Source = "close.png",
				Command = new Command(() =>
				{
					MainPageViewModel.nodes.Remove(node);
				}),
			};

			//grid.Add(layout, 0, 0);
			grid.SetColumn(layout, 0);
			grid.SetRowSpan(layout, 2);
			grid.Add(layout);

			grid.Add(cancelButton, 1, 0);

			border.Content = grid;
			border.Margin = 10;
			border.Padding = 10;
			border.WidthRequest = 600;
			border.HorizontalOptions = LayoutOptions.Start;
			border.StrokeShape = new RoundRectangle
			{
				CornerRadius = new CornerRadius(20, 20, 20, 20)
			};

			Content = border;
		}
	}
}

