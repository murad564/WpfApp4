using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _cacheNum = "";
        private string _memoryNum = "0";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Content)
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        if (label_number.Content.ToString()?.Length < 16)
                        {
                            if (label_number.Content.ToString() == "0")
                                label_number.Content = (string)button.Content;
                            else label_number.Content += (string)button.Content;
                        }
                        break;
                    case "%":
                        label_number.Content = (double.Parse((string)label_number.Content) / 100).ToString();
                        break;
                    case "CE":
                        if (((string)label_cache.Content).Contains("="))
                            label_cache.Content = "0";
                        label_number.Content = "0";
                        break;
                    case "C":
                        label_number.Content = "0";
                        label_cache.Content = "0";
                        break;
                    case "1/x":
                        label_number.Content = (1 / double.Parse((string)label_number.Content)).ToString("F14");
                        Fix();
                        break;
                    case "x²":
                        label_number.Content = Math.Pow(double.Parse((string)label_number.Content), 2).ToString("F14");
                        Fix();
                        break;
                    case "²√x":
                        label_number.Content = Math.Sqrt(double.Parse((string)label_number.Content)).ToString("F14");
                        Fix();
                        break;
                    case "/":
                        AddOperator("/");
                        break;
                    case "x":
                        AddOperator("x");
                        break;
                    case "-":
                        AddOperator("-");
                        break;
                    case "+":
                        AddOperator("+");
                        break;
                    case "+/-":
                        label_number.Content = (-(int.Parse((string)label_number.Content))).ToString();
                        break;
                    case ",":
                        if (((string)label_number.Content).Contains(','))
                            break;
                        label_number.Content += ",";
                        break;
                    case "=":
                        if (label_number.Content.ToString()?.Length < 16)
                            EqualOperation();
                        break;
                    default:
                        if ((string)button.Tag == "del")
                        {

                            if (((string)label_number.Content).Length > 1)
                                label_number.Content = ((string)label_number.Content).Remove(((string)label_number.Content).Length - 1, 1);
                            else if (((string)label_number.Content).Length == 0 || ((string)label_number.Content).Length == 1)
                                label_number.Content = "0";
                        }
                        break;
                }
            }
        }

        private void Fix()
        {
            int f = 14;
            for (int i = ((string)label_number.Content).Length - 1; i >= 0; i--)
            {
                if (((string)label_number.Content).Contains(',') && ((string)label_number.Content)[i] == '0')
                    label_number.Content = (double.Parse((string)label_number.Content)).ToString($"F{--f}");
                else break;
            }
        }

        private bool HasOperation()
        {
            if (((string)label_cache.Content).Last() == '+' || ((string)label_cache.Content).Last() == '-' || ((string)label_cache.Content).Last() == 'x' || ((string)label_cache.Content).Last() == '/')
                return true;
            return false;
        }

        private bool HasEqual()
        {
            if (((string)label_cache.Content).Last() == '=')
                return true;
            return false;
        }

        private void EqualOperation()
        {
            if ((string)label_cache.Content == "0")
                return;
            string firstOp = "";
            if (!HasEqual())
            {
                int i;
                int k = 0;
                if (((string)label_cache.Content).First() == '-')
                {
                    firstOp += "-";
                    k = 1;
                }
                if (((string)label_number.Content).Last() == ',')
                {
                    label_number.Content = ((string)label_number.Content).Remove(((string)label_number.Content).Length - 1, 1);
                }
                for (i = k; i < ((string)label_cache.Content).Length; i++)
                {
                    if (int.TryParse(((string)label_cache.Content)[i].ToString(), out _))
                        firstOp += ((string)label_cache.Content)[i];
                    else if (((string)label_cache.Content)[i].ToString() == ",")
                        firstOp += ",";
                    else break;
                }
                _cacheNum = (string)label_number.Content;
                switch (((string)label_cache.Content)[i])
                {
                    case '+':
                        label_cache.Content += (string)label_number.Content + "=";
                        label_number.Content = (double.Parse(firstOp) + double.Parse((string)label_number.Content)).ToString("F14");
                        break;
                    case '-':
                        label_cache.Content += (string)label_number.Content + "=";
                        label_number.Content = (double.Parse(firstOp) - double.Parse((string)label_number.Content)).ToString("F14");
                        break;
                    case 'x':
                        label_cache.Content += (string)label_number.Content + "=";
                        label_number.Content = (double.Parse(firstOp) * double.Parse((string)label_number.Content)).ToString("F14");
                        break;
                    case '/':
                        label_cache.Content += (string)label_number.Content + "=";
                        if (!((double.Parse(firstOp) / double.Parse((string)label_number.Content)).ToString("F14") == "∞" || !((double.Parse(firstOp) / double.Parse((string)label_number.Content)).ToString("F14") == "-∞")))
                            label_number.Content = (double.Parse(firstOp) / double.Parse((string)label_number.Content)).ToString("F14");
                        else
                        {
                            MessageBox.Show("Cannot divide by zero");
                            _cacheNum = "";
                            label_cache.Content = "0";
                            label_number.Content = "0";
                        }
                        break;
                }
                Fix();
            }
            else
            {
                int i;
                int k = 0;
                if (((string)label_number.Content).First() == '-')
                {
                    firstOp += "-";
                    k = 1;
                }

                for (i = k; i < ((string)label_cache.Content).Length; i++)
                {
                    if (int.TryParse(((string)label_cache.Content)[i].ToString(), out _))
                        continue;
                    else if (((string)label_cache.Content)[i].ToString() == ",")
                        continue;
                    else break;
                }
                label_cache.Content = label_number.Content + ((string)label_cache.Content)[i].ToString();

                for (i = k; i < ((string)label_cache.Content).Length; i++)
                {
                    if (int.TryParse(((string)label_cache.Content)[i].ToString(), out _))
                        firstOp += ((string)label_cache.Content)[i];
                    else if (((string)label_cache.Content)[i].ToString() == ",")
                        firstOp += ",";
                    else break;
                }

                switch (((string)label_cache.Content)[i])
                {
                    case '+':
                        label_cache.Content += _cacheNum;
                        label_number.Content = (double.Parse(firstOp) + double.Parse(_cacheNum)).ToString("F14");
                        break;
                    case '-':
                        label_cache.Content += _cacheNum;
                        label_number.Content = (double.Parse(firstOp) - double.Parse(_cacheNum)).ToString("F14");
                        break;
                    case 'x':
                        label_cache.Content += _cacheNum;
                        label_number.Content = (double.Parse(firstOp) * double.Parse(_cacheNum)).ToString("F14");
                        break;
                    case '/':
                        label_cache.Content += _cacheNum;
                        label_number.Content = (double.Parse(firstOp) / double.Parse(_cacheNum)).ToString("F14");
                        break;
                }
                Fix();
                label_cache.Content += "=";
            }
        }

        private void AddOperator(string op)
        {
            if (HasEqual())
            {
                label_cache.Content = (string)label_number.Content;
            }
            if (!(label_cache.Content.ToString() == "0"))
            {
                if (!HasOperation())
                {
                    label_cache.Content += op;
                    label_number.Content = "0";
                }
                else
                {
                    label_cache.Content = ((string)label_cache.Content).Remove(((string)label_cache.Content).Length - 1, 1);
                    label_cache.Content += op;
                    label_number.Content = "0";
                }
            }
            else if (!(label_number.Content.ToString() == "0"))
            {
                if (((string)label_number.Content).Last() == ',')
                    label_number.Content = ((string)label_number.Content).Remove(((string)label_number.Content).Length - 1, 1);
                label_cache.Content = (string)label_number.Content;
                label_number.Content = "0";
                AddOperator(op);
            }
        }

        private void Button_Click_Memory(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                switch (button.Content)
                {
                    case "M+":
                        if (!mc.IsEnabled && (string)label_number.Content != "0")
                        {
                            mc.IsEnabled = true;
                            mr.IsEnabled = true;
                            ms.IsEnabled = true;
                        }
                        _memoryNum = (double.Parse(_memoryNum) + double.Parse((string)label_number.Content)).ToString();
                        label_number.Content = 0;
                        label_cache.Content = 0;
                        break;
                    case "M-":
                        if (!mc.IsEnabled && (string)label_number.Content != "0")
                        {
                            mc.IsEnabled = true;
                            mr.IsEnabled = true;
                            ms.IsEnabled = true;
                        }
                        _memoryNum = (double.Parse(_memoryNum) - double.Parse((string)label_number.Content)).ToString();
                        label_number.Content = 0;
                        label_cache.Content = 0;
                        break;
                    case "MR":
                        label_number.Content = _memoryNum;
                        break;
                    case "MC":
                        label_number.Content = 0;
                        _memoryNum = "0";
                        mc.IsEnabled = false;
                        mr.IsEnabled = false;
                        ms.IsEnabled = false;
                        break;
                    case "MS":
                        if (!mc.IsEnabled && (string)label_number.Content != "0")
                        {
                            mc.IsEnabled = true;
                            mr.IsEnabled = true;
                            ms.IsEnabled = true;
                        }
                        _memoryNum = (string)label_number.Content;
                        label_cache.Content = 0;
                        break;
                }
            }
        }
    }
}