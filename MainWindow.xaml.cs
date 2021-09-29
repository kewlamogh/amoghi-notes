using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace amoghi_notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    internal class Item
    {
        public string Content;
        public override string ToString()
        {
            return Content;
        }
        public void Delete(ref ItemCollection notes)
        {
            notes.Remove(Content);
        }
    }

    public partial class MainWindow : Window
    {
        private readonly Func<string, string> Format = (string i) => i.Replace("<br>", "\n").Trim();
        private List<Item> items = new();
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            noteText.Text = "";
        }
        private void Load()
        {
            foreach (string i in Persistance.GetAll()) if (Format(i) != "") { notes.Items.Add(Format(i)); items.Add( new Item { Content = Format(i)} ); }
        }

        public MainWindow()
        {
            InitializeComponent();
            Load();
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

            if (notes.Items.Contains(noteText.Text))
            {
                MessageBox.Show("Duplicate not allowed.");
                return;
            }

            TextBox x = noteText;
            notes.Items.Add(Format(x.Text));
            items.Add(new Item { Content = Format(x.Text) });
            x.Text = "";

            Persistance.Save(notes.Items);
        }

        private void Notes_DoubleClick(object sender, EventArgs e)
        {
            /*
             * editing
             */
            try
            {
                ItemCollection o = notes.Items;

                noteText.Text = notes.Items[notes.SelectedIndex].ToString();
                items[notes.SelectedIndex].Delete(ref o);
                Alert.Text = "[EDIT]";
                Persistance.Save(notes.Items);
            } catch
            {
                Console.WriteLine($"The index is: {notes.SelectedIndex}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Persistance.Write( new List<string>() );
            MessageBox.Show("Deleted!");
            notes.Items.Clear();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            IEnumerable<string> x = from y in Persistance.GetAll()
                                    where (y.Contains(query.Text.ToUpper()) || y.Contains(query.Text.ToLower())) && Format(y) != ""
                                    select y;
            results.Items.Clear();
            foreach (string i in x) results.Items.Add(Format(i));
        }
    }
}
