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

namespace Diplom.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddObjectWindow.xaml
    /// </summary>
    public partial class AddObjectWindow : Window
    {
        public AddObjectWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.AdminViewModels.AddObjecttViewModel();
        }
    }
}
