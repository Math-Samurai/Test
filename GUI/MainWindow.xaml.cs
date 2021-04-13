﻿using System;
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
using System.ComponentModel;
using System.Collections.ObjectModel;
using PracticalWork;

namespace GUI
{

    public partial class MainWindow : Window
    {
        protected override void OnClosing(System.ComponentModel.CancelEventArgs args)
        {
            if (currentCol != null) {
                if (currentCol.IsChanged)
                {
                    MessageBoxResult res = MessageBox.Show("Есть несохранённые изменения в коллекции, хотите сохранить их?", "Предупреждение.", MessageBoxButton.YesNoCancel);
                    if (res == MessageBoxResult.Yes)
                    {
                        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                        if (dlg.ShowDialog() == true)
                        {
                            currentCol.Save(dlg.FileName);
                        }
                    } else if (res == MessageBoxResult.No)
                    {
                        base.OnClosing(args);
                    } else
                    {
                        args.Cancel = true;
                    }
                }
            } else
            {
                base.OnClosing(args);
            }
        }
        private V3MainCollection currentCol;
        private CustomDataItem customDataItem;
        private bool isV3Col(object ob)
        {
            return false;
        }
        public MainWindow()
        {
            InitializeComponent();
            customDataItem = new CustomDataItem(new V3DataCollection("q", DateTime.Now));
            ModuleTextBox.DataContext = customDataItem;
            YCoordTextBox.DataContext = customDataItem;
            XCoordTextBox.DataContext = customDataItem;
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
                this.DataContext = currentCol;
                
                Binding bind = new Binding();
                bind.Source = currentCol;
                bind.Path = new PropertyPath("IsChanged");
                bind.StringFormat = "Была ли изменена коллекция с момента своего послежнего сохранения: {0}.";
                IsChangedText.SetBinding(TextBlock.TextProperty, bind);

                Binding bind2 = new Binding();
                bind2.Source = currentCol;
                bind2.Path = new PropertyPath("MaxDistance");
                bind2.StringFormat = "Максимальное расстояние = {0}";
                MaxDistanceText.SetBinding(TextBlock.TextProperty, bind2);
            } catch
            {
                MessageBox.Show("Не удалось создать новую коллекцию.", "Ошибка.");
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
        private void AddDefaultV3DataCollection(object sender, RoutedEventArgs args)
        {
            try
            {
                if (currentCol == null)
                {
                    MessageBox.Show("Сначала создайте коллекцию.", "Ошибка.");
                    return;
                }
                V3DataCollection col = new V3DataCollection("Base", DateTime.Now);
                currentCol.Add(col);
            } catch
            {
                MessageBox.Show("Не удалось добавить элемент в коллекцию.", "Ошибка.");
            }
        }
        private void AddDefaultV3DataOnGrid(object sender, RoutedEventArgs args)
        {
            try
            {
                if (currentCol == null)
                {
                    MessageBox.Show("Сначала создайте коллекцию.", "Ошибка.");
                    return;
                }
                V3DataOnGrid col = new V3DataOnGrid("Base", DateTime.Now, new Grid1D(0.0f, 0), new Grid1D(0.0f, 0));
                currentCol.Add(col);
            } catch
            {
                MessageBox.Show("Не удалось добавить элемент в коллекцию.", "Ошибка.");
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
        private void V3DataColViewFilter(object sender, FilterEventArgs args)
        {
            V3Data item = (V3Data)args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V3DataCollection))
                {
                    args.Accepted = true;
                } else
                {
                    args.Accepted = false;
                }
            }
        }
        private void V3DataOnGridViewFilter(object sender, FilterEventArgs args)
        {
            V3Data item = (V3Data)args.Item;
            if (item != null)
            {
                if (item.GetType() == typeof(V3DataOnGrid))
                {
                    args.Accepted = true;
                }
                else
                {
                    args.Accepted = false;
                }
            }
        }
        private void ModuleTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (V3DataColElements.SelectedItem == null)
            {
                return;
            }
            customDataItem.col = (V3DataCollection)V3DataColElements.SelectedItem;
        }
        private void YCoordTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (V3DataColElements.SelectedItem == null)
            {
                return;
            }
            customDataItem.col = (V3DataCollection)V3DataColElements.SelectedItem;
        }
        private void XCoordTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (V3DataColElements.SelectedItem == null)
            {
                return;
            }
            customDataItem.col = (V3DataCollection)V3DataColElements.SelectedItem;
        }
        private void CommandBinding_Open(object sender, ExecutedRoutedEventArgs e)
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
                        this.DataContext = currentCol;

                        Binding bind3 = new Binding();
                        bind3.Source = currentCol;
                        bind3.Path = new PropertyPath("IsChanged");
                        bind3.StringFormat = "Была ли изменена коллекция с момента своего послежнего сохранения: {0}.";
                        IsChangedText.SetBinding(TextBlock.TextProperty, bind3);

                        Binding bind4 = new Binding();
                        bind4.Source = currentCol;
                        bind4.Path = new PropertyPath("MaxDistance");
                        bind4.StringFormat = "Максимальное расстояние = {0}";
                        MaxDistanceText.SetBinding(TextBlock.TextProperty, bind4);
                    }
                    currentCol.Load(dlg.FileName);
                    currentCol.IsChanged = false;
                    this.DataContext = currentCol;
                }
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить коллекцию.", "Ошибка.");
            }
        }
        private void CommandBinding_CanSave(object sender, CanExecuteRoutedEventArgs e)
        {
            if (currentCol == null)
            {
                e.CanExecute = false;
                return;
            }
            if (!currentCol.IsChanged)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }
        private void CommandBinding_Save(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                if (dlg.ShowDialog() == true)
                {
                    currentCol.Save(dlg.FileName);
                }
            } catch
            {
                MessageBox.Show("Не удалось сохранить коллекцию.", "Ошибка");
            }
        }
        private void CommandBinding_CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            if (allElements.SelectedItem != null)
            {
                e.CanExecute = true;
                return;
            }
            e.CanExecute = false;
        }
        private void CommandBinding_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                V3Data col = (V3Data)allElements.SelectedItem;
                currentCol.Remove(col.Data, col.Time);
            } catch
            {
                MessageBox.Show("Не удалось удалить элемент из коллекции.", "Ошибка.");
            }
        }
        private void CommandBinding_CanAddDataItem(object sender, CanExecuteRoutedEventArgs e)
        {
            if (V3DataColElements.SelectedItem == null)
            {
                e.CanExecute = false;
                return;
            }
            if (Validation.GetHasError(XCoordTextBox) || Validation.GetHasError(YCoordTextBox) || Validation.GetHasError(ModuleTextBox))
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = true;
        }
        private void CommandBinding_AddDataItem(object sender, ExecutedRoutedEventArgs e)
        {
            V3DataCollection col = (V3DataCollection)V3DataColElements.SelectedItem;
            DataItem item = new DataItem(new System.Numerics.Vector2(float.Parse(XCoordTextBox.Text), float.Parse(YCoordTextBox.Text)), double.Parse(ModuleTextBox.Text));
            col.Add(item);
            currentCol.IsChanged = true;
            currentCol.Save("temp");
            currentCol.Load("temp");
            System.IO.File.Delete("temp");
        }
    }
}
