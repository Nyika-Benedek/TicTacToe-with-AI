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
using TicTacToe.AI;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for AiOptionsWindow.xaml
    /// </summary>
    public partial class AiOptionsWindow : Window
    {
        public AiOptionsWindow()
        {
            InitializeComponent();
        }

        public AiLogicType aiLogicType { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (random.IsChecked == true)
            {
                aiLogicType = AiLogicType.Random;
            }

            if (MinMax.IsChecked == true)
            {
                aiLogicType = AiLogicType.MinMax;
            }
            Close();
        }
    }
}
