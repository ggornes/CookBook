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
    /// Interaction logic for AddMeasureView.xaml
    /// </summary>
    public partial class AddMeasureView : Window
    {
        public AddMeasureView(ViewModel.AddMeasureViewModel AddMeasureVM)
        {
            InitializeComponent();
            DataContext = AddMeasureVM;
        }

        public AddMeasureView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
