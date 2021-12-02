using System;
using System.Globalization;

using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;
using FirstWpfApp.Models;
using FirstWpfApp.ViewModels;
using LiveCharts;

namespace FirstWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            int biggerFont = 32;
            int smallerFont = 15;
            int radioFont = 18;

            layoutGrid.Background = new SolidColorBrush(Color.FromRgb(58, 0x8b, 0xED));
            
            DecorateTextBlock(MethodNameTextBlock, biggerFont, false);
            MethodNameTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
            
            DecorateTextBlock(ChooseFunctionTextBlock, biggerFont, false);
            DecorateTextBlock(BoundsTextBlock, biggerFont, false);
            
            DecorateTextBlock(LeftBoundTextBlock, smallerFont, false);
            DecorateTextBlock(RightBoundTextBlock, smallerFont, false);
            
            DecorateTextBlock(AccuracyTextBlock, smallerFont, false);
            DecorateTextBlock(minumumTextBlock, smallerFont, true);
            DecorateTextBlock(resultTextBlock, smallerFont, true);
            
            DecorateButton(PlotButton, new SolidColorBrush(Color.FromRgb(58, 0x8B, 0xED)));
            DecorateButton(ResetButton, new SolidColorBrush(Color.FromRgb(0xFF, 63, 47)));

            DecorateRadioButton(FunctionRadioButton1, radioFont);
            DecorateRadioButton(FunctionRadioButton2, radioFont);
            DecorateRadioButton(FunctionRadioButton3, radioFont);
            DecorateRadioButton(FunctionRadioButton4, radioFont);

            Chart.Zoom = ZoomingOptions.Xy;
        }

        private void DecorateTextBlock(TextBlock textBlock, int fontSize, bool doCenterText)
        {
            textBlock.FontFamily = new FontFamily("Roboto");
            textBlock.FontSize = fontSize;
            textBlock.SetValue(TextBlock.FontWeightProperty, FontWeights.Medium);
            textBlock.TextWrapping = TextWrapping.Wrap;

            if (doCenterText)
            {
                textBlock.TextAlignment = TextAlignment.Center;
            }
            else
            {
                textBlock.TextAlignment = TextAlignment.Left;
            }
        }

        private void DecorateButton(Button button, Brush color)
        {
            button.FontFamily = new FontFamily("Roboto");
            button.FontSize = 15;
            button.Foreground = Brushes.White;
            button.SetValue(TextBlock.FontWeightProperty, FontWeights.Medium);
            button.Background = color;
            button.BorderBrush = color;
        }

        private void DecorateRadioButton(RadioButton radioButton, int fontSize)
        {
            radioButton.FontFamily = new FontFamily("Roboto");
            radioButton.FontSize = fontSize;
            radioButton.Height = 30;
        }

        private void PlotButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
