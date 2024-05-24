using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace VKRTalalaev.ClassFolder
{
    class MBClass
    {
        public static void ErrorMB(string text)
        {
            MessageBox.Show(text, "Ошибка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static void ErrorMB(Exception exception)
        {
            MessageBox.Show(exception.Message, "Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static void InfoMB(string text)
        {
            MessageBox.Show(text, "Информация",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public static bool QuestionMB(string text)
        {
            return MessageBoxResult.Yes == MessageBox.Show(text, "Вопрос",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Question);
        }

        public static void ExitMB()
        {
            bool result = QuestionMB("Вы действительно желаете выйти?");
            if (result == true)
            {
                App.Current.Shutdown();
            }
        }

        public static void ShowErrorPopup(string errorMessage, Window parentWindow)
        {
            Popup errorPopup = new Popup
            {
                StaysOpen = false,
                Placement = PlacementMode.Relative,
                PlacementTarget = parentWindow,
                VerticalOffset = parentWindow.ActualHeight + 10
            };

            Border border = new Border
            {
                CornerRadius = new CornerRadius(5),
                Background = new LinearGradientBrush
                {
                    EndPoint = new Point(0.5, 1),
                    StartPoint = new Point(0.5, 0),
                    GradientStops =
                    {
                        new GradientStop(Colors.Black, 0),
                        new GradientStop(Color.FromRgb(247, 4, 82), 1),
                        new GradientStop(Color.FromRgb(255, 121, 40), 0),
                        new GradientStop(Color.FromRgb(255, 121, 40), 0.235),
                        new GradientStop(Color.FromRgb(255, 129, 42), 0.091)
                    }
                }
            };

            TextBlock textBlock = new TextBlock
            {
                Margin = new Thickness(10),
                Foreground = Brushes.White,
                Text = "Ошибка: " + errorMessage
            };

            border.Child = textBlock;
            errorPopup.Child = border;


            errorPopup.Placement = PlacementMode.Relative;
            errorPopup.IsOpen = true;
        }

        public static void ShowMesagePopup(string errorMessage, Window parentWindow)
        {
            Popup errorPopup = new Popup
            {
                StaysOpen = false,
                Placement = PlacementMode.Relative,
                PlacementTarget = parentWindow,
                VerticalOffset = parentWindow.ActualHeight + 10
            };

            Border border = new Border
            {
                CornerRadius = new CornerRadius(5),
                Background = new LinearGradientBrush
                {
                    EndPoint = new Point(0.5, 1),
                    StartPoint = new Point(0.5, 0),
                    GradientStops =
                    {
                            new GradientStop(Colors.Blue, 0),
                            new GradientStop(Colors.Cyan, 1)
                    }
                }
            };

            TextBlock textBlock = new TextBlock
            {
                Margin = new Thickness(10),
                Foreground = Brushes.White,
                Text = "Сообщение: " + errorMessage
            };

            border.Child = textBlock;
            errorPopup.Child = border;


            errorPopup.Placement = PlacementMode.Relative;
            errorPopup.IsOpen = true;
        }
    }
}
