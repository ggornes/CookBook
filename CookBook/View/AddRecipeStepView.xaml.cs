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
using System.Windows.Shapes;

namespace CookBook.View
{
    /// <summary>
    /// Interaction logic for AddRecipeStepView.xaml
    /// </summary>
    public partial class AddRecipeStepView : Window
    {
        public AddRecipeStepView()
        {
            InitializeComponent();
        }

        public AddRecipeStepView(ViewModel.AddRecipeStepViewModel addRecipeStepVM)
        {
            InitializeComponent();
            DataContext = addRecipeStepVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
