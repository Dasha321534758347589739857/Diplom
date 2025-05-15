using CommunityToolkit.Mvvm.Messaging;
using Diplom.Data;
using Diplom.Data.DB;
using Diplom.Message;
using Diplom.ViewModels.ViewModelsData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для EditKey.xaml
    /// </summary>
    public partial class EditKey : Window
    {
        List<Point> points = new List<Point>();
        Line redLine = new Line();
        Line redLine1 = new Line();
        Line redLine2 = new Line();
        public EditKey()
        {
            InitializeComponent();

            using Context ctx = new Context();
            
            

        }


        private void Button1_Checked(object sender, RoutedEventArgs e)
        {
            Line redLine = new Line();
            Point s = new Point();
            s = Button1.TransformToAncestor(this).Transform(new Point(Button1.ActualWidth / 2, Button1.ActualHeight / 2));
            points.Add(s);

        }
        private void Button2_Checked(object sender, RoutedEventArgs e)
        {
            Line redLine = new Line();
            Point s = new Point();
            s = Button2.TransformToAncestor(this).Transform(new Point(Button2.ActualWidth / 2, Button2.ActualHeight / 2));
            points.Add(s);

        }
        private void Button3_Checked(object sender, RoutedEventArgs e)
        {
            Line redLine = new Line();
            Point s = new Point();
            s = Button3.TransformToAncestor(this).Transform(new Point(Button3.ActualWidth / 2, Button3.ActualHeight / 2));
            points.Add(s);

        }
        private void Button4_Checked(object sender, RoutedEventArgs e)
        {
            Line redLine = new Line();
            Point s = new Point();
            s = Button4.TransformToAncestor(this).Transform(new Point(Button4.ActualWidth / 2, Button4.ActualHeight / 2));
            points.Add(s);
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            points.Clear();
            Button1.IsChecked = false;
            Button2.IsChecked = false;
            Button3.IsChecked = false;
            Button4.IsChecked = false;
            wrapPanel1.Children.Remove(redLine);
            wrapPanel1.Children.Remove(redLine1);
            wrapPanel1.Children.Remove(redLine2);
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            if (points.Count != 4)
            {
                MessageBox.Show("Неправильно введён графический ключ. Повторите попытку");
                points.Clear();
                Button1.IsChecked = false;
                Button2.IsChecked = false;
                Button3.IsChecked = false;
                Button4.IsChecked = false;



            }
            else
            {
                
                redLine.X1 = points[0].X;
                redLine.Y1 = points[0].Y;
                redLine.X2 = points[1].X;
                redLine.Y2 = points[1].Y;
                SolidColorBrush redBrush = new SolidColorBrush();
                redBrush.Color = Colors.Green;
                redLine.StrokeThickness = 3;
                redLine.Stroke = redBrush;
                wrapPanel1.Children.Add(redLine);
               
                redLine1.X1 = points[1].X;
                redLine1.Y1 = points[1].Y;
                redLine1.X2 = points[2].X;
                redLine1.Y2 = points[2].Y;
                redLine1.StrokeThickness = 3;
                redLine1.Stroke = redBrush;
                wrapPanel1.Children.Add(redLine1);
               
                redLine2.X1 = points[2].X;
                redLine2.Y1 = points[2].Y;
                redLine2.X2 = points[3].X;
                redLine2.Y2 = points[3].Y;
                redLine2.StrokeThickness = 3;
                redLine2.Stroke = redBrush;
                wrapPanel1.Children.Add(redLine2);

                string pointsAdmins = "";
                for (int i = 0; i < points.Count(); i++)
                {

                    pointsAdmins += points[i].X.ToString() + " ";
                    pointsAdmins += points[i].Y.ToString() + " ";
                }
                StringBuilder builder = new StringBuilder();

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(pointsAdmins));

                    foreach (byte b in bytes)
                    {
                        builder.Append(b.ToString("x2")); // Convert to hexadecimal string
                    }

                }

                //string s=AesShifr.Encrypt(pointsAdmins);
                //string hj = AesShifr.Decrypt(s);

                

                using Context ctx = new Context();
                ctx.Point.RemoveRange(ctx.Point);
                ctx.SaveChanges();
                
                //WeakReferenceMessenger.Default.Send(new NewGrafDateMessage(new NewKey() { Points = pointsAdmins.GetHashCode().ToString()}));
                GrafDate grafDate = new GrafDate() { Points = builder.ToString() };
                ctx.Point.Add(grafDate);
                ctx.SaveChanges();

                MessageBox.Show("Графический ключ успешно заменен");

                
            }
        }
    }
}
