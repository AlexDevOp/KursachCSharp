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

namespace Kursach
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class KeyPopup : Window
    {
        public KeyPopup()
        {
            InitializeComponent();
        }

        public string GetKey()
        {
            return KeyField.Text;
        }

        private void OnInputButtonClicked(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(KeyField.Text))
            {
                MessageBox.Show(this, "Введите ключ!!!");
                return;
            }

            this.Close();
        }
    }
}
