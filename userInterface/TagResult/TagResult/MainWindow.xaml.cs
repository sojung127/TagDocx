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

namespace TagResult
{

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void GoToMain_Click(object sender, RoutedEventArgs e)
        {
          
        }


    }

    class TagData {
        public string Name { get; set; }
        public string Form { get; set; }
        public string Contents { get; set; }

        private static List<TagData> instance;

        private static List<TagData> GetInstance() {
            if (instance == null)
                instance = new List<TagData>();

            return instance;
        }
    }
}
