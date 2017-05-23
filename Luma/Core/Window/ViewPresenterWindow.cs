using System;
using System.IO;
using System.Reflection;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.VisualStudio.PlatformUI;
using Seth.Luma.Core.View;
using Seth.Luma.Core.ViewModel;
using Seth.Luma.Core.ViewModel.Interfaces;

namespace Seth.Luma.Core.Window
{
    /// <summary>
    /// Window for presenting a view model
    /// </summary>
    public class ViewPresenterWindow : MetroWindow
    {
        /// <summary>
        /// DialogWindowViewModel
        /// </summary>
        private readonly IDialogWindowViewModel _dialogWindowViewModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewModel"></param>
        public ViewPresenterWindow(ViewModelBase viewModel)
        {
            Content = ViewResolver.CreateView(viewModel);

            if (Content == null)
            {
                throw new ArgumentException("Failed to create view.", nameof(viewModel));
            }

            _dialogWindowViewModel = viewModel as IDialogWindowViewModel;
            if (_dialogWindowViewModel != null)
            {
                _dialogWindowViewModel.RequestCloseWindow += OnRequestCloseWindow;
            }

            Closed += OnClosed;

            Style = Application.Current.Resources["VSWindowStyleKey"] as Style;

            Owner = Application.Current.Windows[0];
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // We need to load the assembly, because otherwise wpf wound find the needed resources 
            var mahApps = Assembly.Load("MahApps.Metro");
            if (mahApps != null)
            {
                var resourceDictionary = new ResourceDictionary();

                resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml")
                });
                resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml")
                });
                resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml")
                });
                resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/VS/Colors.xaml")
                });
                resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/VS/Styles.xaml")
                });
                resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/FlatButton.xaml")
                });
                
                if (VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey).GetBrightness() <= 0.2)
                {
                    resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Cyan.xaml")
                    });
                    resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml")
                    });
                }
                else
                {
                    resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Cyan.xaml")
                    });
                    resourceDictionary.MergedDictionaries.Add(new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml")
                    });
                }

                Resources = resourceDictionary;
            }
            else
            {
                throw new FileNotFoundException("Resources couldn't be found.");
            }
        }

        /// <summary>
        /// Window closed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OnClosed(Object sender, EventArgs e)
        {
            if (_dialogWindowViewModel != null)
            {
                _dialogWindowViewModel.RequestCloseWindow += OnRequestCloseWindow;
            }
        }

        /// <summary>
        /// Close window is requested
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OnRequestCloseWindow(Object sender, EventArgs e)
        {
            Close();
        }
    }
}
