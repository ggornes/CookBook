using CookBook.ViewModel;
using CookBookData.Model.DbActions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CookBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow app = new MainWindow();
            DbActions dbActions = new DbActions();
            MainWindowViewModel context = new MainWindowViewModel(dbActions);
            app.DataContext = context;
            app.Show();
        }
    }
}
