using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace amoghi_notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            noteText.Text = "";
        }
        public MainWindow()
        {
            InitializeComponent();
            foreach (string i in Persistance.GetAll())
            {
                if (i.Trim() != "") notes.Items.Add(i.Trim());
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            string was = Alert.Text;
            Alert.Text = "";

            Persistance.Save(notes.Items);

            if (noteText.Text == "")
            {
                if (was == "[EDIT]") { _ = MessageBox.Show("Deleted!"); };
                return;
            }


            TextBox x = noteText;
            notes.Items.Add(x.Text);
            x.Text = "";

            Persistance.Save(notes.Items);
        }

        private void Notes_DoubleClick(object sender, EventArgs e)
        {

            noteText.Text = notes.Items[notes.SelectedIndex].ToString();
            notes.Items.Remove(notes.Items[notes.SelectedIndex]);
            Alert.Text = "[EDIT]";

            Persistance.Save(notes.Items);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Persistance.Write( new List<string>() );
            MessageBox.Show("Deleted!");
            notes.Items.Clear();
        }

        private void y_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<string> x = from y in Persistance.GetAll()
                                    where y.Contains(query.Text)
                                    select y;
            results.Items.Clear();
            foreach (var i in x)
            {
                results.Items.Add(i);
            }
        }
    }
}
