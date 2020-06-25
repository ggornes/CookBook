using CookBook.ViewModel;
using CookBook.ViewModel.Interfaces;
using CookBookData.Model.DbActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IViewModel
    {
        public MainWindow()
        {
            InitializeComponent();
            DbActions dbActions = new DbActions();
            DataContext = new MainWindowViewModel(dbActions);
        }

        private void closeMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
