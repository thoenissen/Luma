using System;
using System.Windows;
using Seth.Luma.Core.Diagnostics;
using Seth.Luma.Core.ViewModel;

namespace Seth.Luma.Core.View
{
    /// <summary>
    /// Resolving views of view models
    /// </summary>
    public class ViewResolver
    {
        /// <summary>
        /// Creates the view of the view model
        /// </summary>
        /// <param name="viewModel"></param>
        public static FrameworkElement CreateView(ViewModelBase viewModel)
        {
            FrameworkElement view = null;

            if (viewModel != null)
            {
                try
                {
                    var viewName = viewModel.GetType().FullName.Replace(".ViewModel.", ".View.");

                    if (viewName.EndsWith("ViewModel"))
                    {
                        viewName = viewName.Remove(viewName.Length - "Model".Length);
                    }

                    var viewType = Type.GetType(viewName);

                    if (viewType != null)
                    {
                        view = (FrameworkElement) Activator.CreateInstance(viewType);

                        view.DataContext = viewModel;
                    }
                }
                catch (Exception ex)
                {
                    DebugListener.WriteToTrace(ex);
                }
            }

            return view;
        }
    }
}
