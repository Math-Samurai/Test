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
using PracticalWork;

namespace GUI
{
    public partial class MainWindow : Window
    {
        private V3MainCollection currentCol;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void New(object sender, RoutedEventArgs args)
        {
            try
            {
                if (currentCol != null && currentCol.IsChanged)
                {
                    MessageBoxResult res = MessageBox.Show("В коллекции есть несохранённые изменения, хотите сохранить их?", "Предупреждение.", MessageBoxButton.YesNoCancel);
                    if (res == MessageBoxResult.Yes)
                    {
                        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                        if (dlg.ShowDialog() == true)
                        {
                            currentCol.Save(dlg.FileName);
                        }
                    } else if (res == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                currentCol = new V3MainCollection();
                allElements.DataContext = currentCol;
            } catch
            {
                MessageBox.Show("Не удалось создать новую коллекцию.", "Ошибка.");
            }
        }
        private void Save(object sender, RoutedEventArgs args)
        {
            try
            {
                if (currentCol == null)
                {
                    MessageBox.Show("Нет открытой коллекции.", "Ошибка.");
                    return;
                }
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    currentCol.Save(dlg.FileName);
                }
            } catch
            {
                MessageBox.Show("Не удалось сохранить коллекцию.", "Ошибка.");
            }
        }
        private void Open(object sender, RoutedEventArgs args)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (currentCol != null && currentCol.IsChanged)
                {
                    MessageBoxResult res = MessageBox.Show("В коллекции есть несохранённые изменения, хотите сохранить их?", "Предупреждение.", MessageBoxButton.YesNoCancel);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (dlg.ShowDialog() == true)
                        {
                            currentCol.Save(dlg.FileName);
                        }
                    }
                    else if (res == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                if (dlg.ShowDialog() == true)
                {
                    if (currentCol == null)
                    {
                        currentCol = new V3MainCollection();
                    }
                    currentCol.Load(dlg.FileName);
                    allElements.DataContext = currentCol;
                }
            } catch
            {
                MessageBox.Show("Не удалось загрузить коллекцию.", "Ошибка.");
            }
        }
        private void AddDefaults(object sender, RoutedEventArgs args)
        {
            try
            {
                if (currentCol == null)
                {
                    MessageBox.Show("Сначала создайте новую коллекцию.", "Ошибка.");
                    return;
                }
                currentCol.AddDefaults();
            } catch
            {
                MessageBox.Show("Не удалось добавить элементы по умолчанию в коллекцию.", "Ошибка.");
            }
        }
        private void AddFromFile(object sender, RoutedEventArgs args)
        {
            try
            {
                if (currentCol == null)
                {
                    MessageBox.Show("Сначала создайте новую коллекцию.", "Ошибка.");
                    return;
                }
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    V3DataCollection col = new V3DataCollection(dlg.FileName);
                    currentCol.Add(col);
                }
            } catch
            {
                MessageBox.Show("Ошибка при чтении из файла.", "Ошибка.");
            }
        }
    }
}
