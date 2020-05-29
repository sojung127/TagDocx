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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TagDocx
{
    /// <summary>
    /// ToastMessage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ToastMessage : Window
    {
        public ToastMessage()
        {
            InitializeComponent();

            this.AllowsTransparency = true;
            this.Topmost = true;

            if (Application.Current.Properties["TM_Msg"] != null)
            {
                TB_Msg.Text = Application.Current.Properties["TM_Msg"].ToString();
            }

            this.Visibility = Visibility.Visible;
            DoubleAnimation dba1 = new DoubleAnimation();
            dba1.From = 0;
            dba1.To = 3;
            dba1.Duration = new Duration(TimeSpan.FromSeconds(0.8));
            dba1.AutoReverse = true;

            dba1.Completed += (s, a) =>
            {
                Close();
            };
            this.BeginAnimation(OpacityProperty, dba1);
        }
    }
}
