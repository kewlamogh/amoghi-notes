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
        private string SelectedItem;
        private bool _IsItemSelected;
        private bool IsItemSelected { get { return _IsItemSelected; } set {
                _IsItemSelected = value;
                BtnAdd.Content = $"{(value ? "Edit" : "Add")} Note";
            } }

        private readonly Func<string, string> Format = (string i) => i.Replace("<br>", "\n").Trim();
        private List<Item> items = new();
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            noteText.Text = "";
        }
        private void Refr(object sender, RoutedEventArgs e)
        {
            notes.Items.Clear();
            items.Clear();

            Load();
            SearchFunctionality();
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

            IsItemSelected = false;

            if (noteText.Text == "")
            {
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

                SelectedItem = notes.Items[notes.SelectedIndex].ToString();
                noteText.Text = SelectedItem;

                IsItemSelected = true;

                notes.Items.Remove(SelectedItem);
                Alert.Text = "[EDIT]";
                Persistance.Save(notes.Items);

            } catch
            {
                Console.WriteLine($"The index is: {notes.SelectedIndex}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Persistance.Write(Persistance.GetAll(), "Recover.txt");

            Persistance.Write(Array.Empty<string>(), "Persistor.txt");
            MessageBox.Show("Deleted all notes [to recover them, go to Recover.txt, copy paste the contents into Persistor.txt, then click the refresh button].");
            notes.Items.Clear();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            SearchFunctionality();
        }
        private void SearchFunctionality()
        {
            IEnumerable<string> x = from y in Persistance.GetAll()
                                    where (y.Contains(query.Text.ToUpper()) || y.Contains(query.Text.ToLower())) && Format(y) != ""
                                    select y;
            results.Items.Clear();
            foreach (string i in x) results.Items.Add(Format(i));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (IsItemSelected)
            {
                notes.Items.Remove(SelectedItem);
                noteText.Text = "";
                Alert.Text = "";

                MessageBox.Show($"Deleted \"{SelectedItem}\"");

                IsItemSelected = false;
            }
        }
        private void Notes_DoubleClick2(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemCollection o = notes.Items;

                SelectedItem = results.Items[results.SelectedIndex].ToString();
                noteText.Text = SelectedItem;

                IsItemSelected = true;

                notes.Items.Remove(SelectedItem);
                Alert.Text = "[EDIT]";
                Persistance.Save(notes.Items);

            }
            catch
            {
                Console.WriteLine($"The index is: {notes.SelectedIndex}");
            }
        }
    }

}
