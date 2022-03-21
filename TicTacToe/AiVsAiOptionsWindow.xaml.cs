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
    /// Interaction logic for AiVsAiOptionsWindow.xaml
    /// </summary>
    public partial class AiVsAiOptionsWindow : Window
    {
        public AiVsAiOptionsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// How AI1 should think.
        /// </summary>
        public AiLogicType ai1LogicType { get; private set; }

        /// <summary>
        /// How AI1 should think.
        /// </summary>
        public AiLogicType ai2LogicType { get; private set; }

        /// <summary>
        /// Gather all input information into an accessible form.
        /// </summary>
        /// <param name="sender">The interacted object.</param>
        /// <param name="e">Data of the mouse related event.</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Ai1Random.IsChecked == true)
            {
                ai1LogicType = AiLogicType.Random;
            }

            if (Ai1MinMax.IsChecked == true)
            {
                ai1LogicType = AiLogicType.MinMax;
            }

            if (Ai2Random.IsChecked == true)
            {
                ai2LogicType = AiLogicType.Random;
            }

            if (Ai2MinMax.IsChecked == true)
            {
                ai2LogicType = AiLogicType.MinMax;
            }
            Close();
        }
    }
}
