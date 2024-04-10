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

namespace CalculatorWindows
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool hayOperador = false;
        private string operador;
        private bool esDecimal = false;
        private bool resultadoDecimal = false;
        private bool hayOperadorPendiente = false;
        private bool hayResultado = false;
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded; // Asignar el manejador de eventos Loaded

            //Cuenta.LostFocus += MiTextBox_LostFocus;
            Cuenta.Focus();

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Cuenta.Focus(); // Establecer el foco en el TextBox deseado al cargar la ventana
        }

        /*private void MiTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Cuenta.Focus(); // Volver a establecer el foco en el TextBox deseado cuando pierda el foco
        }*/

        private void AgregarNumero(String numero)
        {

            if (Cuenta.Text.Equals("0"))
            {
                Cuenta.Text = numero;
                if (!hayResultado) resultadoDecimal = false;
            }
            else
            {
                if (hayOperador || hayResultado)
                {
                    Cuenta.Text = numero;
                    if (hayOperador) hayOperadorPendiente = true;
                    hayResultado = false;
                    hayOperador = false;
                    esDecimal = false;
                }
                else
                {
                    Cuenta.Text = Cuenta.Text + numero;
                }
            }
        }
        private void Cuenta_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key.Equals(Key.Enter))
            {
                RealizarCuenta();
            }
            else if (e.Key.Equals(Key.Back))
            {
                BorrarUltimoDigito();
            }
            else if (e.Key.Equals(Key.OemComma))
            {
                AgregarComa();
            }
            else if (e.Key.Equals(Key.Delete))
            {
                BorrarHistorialYCuenta();
            }
            else if (e.Key.Equals(Key.Divide))
            {
                AgregarOperador("÷");
            }
            else if (e.Key.ToString()[0] == 'D')
            {
                AgregarNumero((e.Key.ToString()[1]).ToString());
            }
            else if (e.Key.Equals(Key.Multiply))
            {
                AgregarOperador("×");
            }
            else if (e.Key.Equals(Key.Subtract))
            {
                AgregarOperador("-");
            }
            else if (e.Key.Equals(Key.Add))
            {
                AgregarOperador("+");
            }

        }
        /*private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verificar si el carácter introducido no es un número
            e.Handled = true;
            if (char.IsDigit(e.Text, e.Text.Length-1))
            {
                AgregarNumero(e.Text);
            } else if (e.Text.Equals("/") || e.Text.Equals("*") || e.Text.Equals("+") || e.Text.Equals("-"))
            {
                AgregarOperador(e.Text.ToString());
            } else if (e.Text.Equals(","))
            {
                AgregarComa();
            } else if (e.Text.Equals("\r"))
            {
                RealizarCuenta();
            }
        }*/


        /*private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Establecer un tamaño de fuente máximo y mínimo
            double maxFontSize = 32;
            double minFontSize = 22;

            // Número máximo de caracteres antes de reducir el tamaño de la fuente
            int maxCharsBeforeShrink = 10;

            // Si el número de caracteres excede el límite, reducir el tamaño de la fuente
            if (textBox.Text.Length > maxCharsBeforeShrink)
            {
                if (textBox.FontSize > minFontSize)
                {
                    textBox.FontSize -= 1;
                } 
            }
            else
            {
                // Si el número de caracteres está dentro del límite, restaurar el tamaño de la fuente máximo
                if (textBox.FontSize < maxFontSize)
                {
                    textBox.FontSize += 1;
                }
            }
        }*/




        private void BorrarHistorialYCuenta()
        {

            Historial.Text = "";
            Cuenta.Text = "0";

            hayOperador = false;
            operador = "";
            esDecimal = false;
            resultadoDecimal = false;
            hayOperadorPendiente = false;

        }

        private void AgregarOperador(Object sender, RoutedEventArgs e)
        {
            var boton = (Button)sender;
            if (!hayOperador)
            {
                if (hayOperadorPendiente)
                {
                    RealizarCuentaUltimoOperador(boton.Content.ToString());
                    hayResultado = true;
                }
                Historial.Text = Cuenta.Text + boton.Content.ToString();
                operador = boton.Content.ToString();
                hayOperador = true;
            }
            else
            {
                Historial.Text = Cuenta.Text + boton.Content.ToString();
                operador = boton.Content.ToString();
            }

        }

        private void AgregarOperador(String op)
        {
            if (!hayOperador)
            {
                if (hayOperadorPendiente)
                {
                    RealizarCuentaUltimoOperador(operador);
                    hayResultado = true;
                }
                Historial.Text = Cuenta.Text + op;
                hayOperador = true;
            }
            else
            {
                Historial.Text = Cuenta.Text + op;
            }
            operador = op;
        }

        private void BorrarUltimoDigito()
        {
            if (!Cuenta.Text.Equals("")) Cuenta.Text = Cuenta.Text.Remove(Cuenta.Text.Length - 1);
        }

        private void RealizarCuentaUltimoOperador(String op)
        {
            String operador1, operador2;
            int resultado = 0;
            double resultadoDouble = 0;

            if (Historial.Text.Equals(""))
            {
                operador1 = Cuenta.Text;
            }
            else
            {
                operador1 = Historial.Text.Remove(Historial.Text.Length - 1);
            }


            operador2 = Cuenta.Text;
            if (!resultadoDecimal)
            {
                switch (operador)
                {
                    case "+":
                        resultado = Int32.Parse(operador1) + Int32.Parse(operador2);
                        Cuenta.Text = resultado.ToString();

                        break;
                    case "-":
                        resultado = Int32.Parse(operador1) - Int32.Parse(operador2);
                        Cuenta.Text = resultado.ToString();

                        break;
                    case "×":
                        resultado = Int32.Parse(operador1) * Int32.Parse(operador2);
                        Cuenta.Text = resultado.ToString();

                        break;
                    case "÷":
                        if (Int32.Parse(operador2) == 0)
                        {
                            Cuenta.Text = "No se puede dividir entre 0.";
                        }
                        else if (Int32.Parse(operador1) % Int32.Parse(operador2) == 0)
                        {
                            resultado = Int32.Parse(operador1) / Int32.Parse(operador2);
                            Cuenta.Text = resultado.ToString();
                        }
                        else
                        {
                            resultadoDouble = Double.Parse(operador1) / Double.Parse(operador2);
                            Cuenta.Text = resultadoDouble.ToString();
                            resultadoDecimal = true;
                        }

                        break;
                }
                Historial.Text = resultado + op;
            }
            else
            {
                switch (operador)
                {
                    case "+":
                        resultadoDouble = Double.Parse(operador1) + Double.Parse(operador2);
                        Cuenta.Text = resultadoDouble.ToString();

                        break;
                    case "-":
                        resultadoDouble = Double.Parse(operador1) - Double.Parse(operador2);
                        Cuenta.Text = resultadoDouble.ToString();

                        break;
                    case "×":
                        resultadoDouble = Double.Parse(operador1) * Double.Parse(operador2);
                        Cuenta.Text = resultadoDouble.ToString();

                        break;
                    case "÷":
                        if (Double.Parse(operador2) == 0)
                        {
                            Cuenta.Text = "No se puede dividir entre 0.";
                        }
                        else
                        {
                            resultadoDouble = Double.Parse(operador1) / Double.Parse(operador2);
                            Cuenta.Text = resultadoDouble.ToString();
                        }

                        break;
                }
                Historial.Text = resultadoDouble + op;
            }


        }

        private void RealizarCuenta()
        {
            String operador1, operador2;
            int resultado;
            double resultadoDouble;
            hayResultado = true;

            if (Historial.Text.Equals(""))
            {
                operador1 = Cuenta.Text;
            }
            else
            {
                operador1 = Historial.Text.Remove(Historial.Text.Length - 1);
            }


            operador2 = Cuenta.Text;
            if (!resultadoDecimal)
            {
                switch (operador)
                {
                    case "+":
                        resultado = Int32.Parse(operador1) + Int32.Parse(operador2);
                        Cuenta.Text = resultado.ToString();

                        break;
                    case "-":
                        resultado = Int32.Parse(operador1) - Int32.Parse(operador2);
                        Cuenta.Text = resultado.ToString();

                        break;
                    case "×":
                        resultado = Int32.Parse(operador1) * Int32.Parse(operador2);
                        Cuenta.Text = resultado.ToString();

                        break;
                    case "÷":
                        if (Int32.Parse(operador2) == 0)
                        {
                            Cuenta.Text = "No se puede dividir entre 0.";
                        }
                        else if (Int32.Parse(operador1) % Int32.Parse(operador2) == 0)
                        {
                            resultado = Int32.Parse(operador1) / Int32.Parse(operador2);
                            Cuenta.Text = resultado.ToString();
                        }
                        else
                        {
                            resultadoDouble = Double.Parse(operador1) / Double.Parse(operador2);
                            Cuenta.Text = resultadoDouble.ToString();
                            resultadoDecimal = true;
                        }

                        break;
                }
            }
            else
            {
                switch (operador)
                {
                    case "+":
                        resultadoDouble = Double.Parse(operador1) + Double.Parse(operador2);
                        Cuenta.Text = resultadoDouble.ToString();

                        break;
                    case "-":
                        resultadoDouble = Double.Parse(operador1) - Double.Parse(operador2);
                        Cuenta.Text = resultadoDouble.ToString();

                        break;
                    case "×":
                        resultadoDouble = Double.Parse(operador1) * Double.Parse(operador2);
                        Cuenta.Text = resultadoDouble.ToString();

                        break;
                    case "÷":
                        if (Double.Parse(operador2) == 0)
                        {
                            Cuenta.Text = "No se puede dividir entre 0.";
                        }
                        else
                        {
                            resultadoDouble = Double.Parse(operador1) / Double.Parse(operador2);
                            Cuenta.Text = resultadoDouble.ToString();
                        }

                        break;
                }
            }
            hayOperadorPendiente = false;
            Historial.Text = Historial.Text + operador2 + "=";
        }

        private void AgregarComa()
        {
            if (hayOperador || hayResultado)
            {
                Cuenta.Text = "0,";
                if (hayOperador) hayOperadorPendiente = true;
                hayResultado = false;
                hayOperador = false;
                esDecimal = false;
            }
            else if (!Cuenta.Text.ToString().Contains(",") || !esDecimal)
            {
                Cuenta.Text = Cuenta.Text + ",";
                resultadoDecimal = true;
                esDecimal = true;
            }
        }

        private void OperacionDirecta(object sender, RoutedEventArgs e)
        {
            var boton = (Button)sender;

            String operador1, operador2;
            if (Historial.Text.Equals(""))
            {
                operador1 = Cuenta.Text;
            }
            else
            {
                operador1 = Historial.Text.Remove(Historial.Text.Length - 1);
            }

            operador2 = Cuenta.Text;

            int resultadoInt = 0;
            double resultadoDouble = 0;

            switch (boton.Content.ToString())
            {
                case "1/x":
                    try
                    {
                        resultadoDouble = (1 / Double.Parse(operador2));
                    }
                    catch (DivideByZeroException ex)
                    {
                        Cuenta.Text = "No se pude dividir entre 0";
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());

                    }
                    resultadoDecimal = true;
                    Historial.Text = "1/(" + operador2 + ")=";
                    break;
                case "x²":
                    if (!resultadoDecimal)
                    {
                        resultadoInt = Int32.Parse(operador2) * Int32.Parse(operador2);
                    }
                    else
                    {
                        resultadoDouble = Double.Parse(operador2) * Double.Parse(operador2);
                    }
                    Historial.Text = "sqr(" + operador2 + ")";

                    break;
                case "²√x":
                    if (Double.Parse(operador2) >= 0)
                    {
                        resultadoDouble = Math.Sqrt(Double.Parse(operador2));
                        Historial.Text = "sqr(" + operador2 + ")";
                        resultadoDecimal = true;
                    }
                    else
                    {
                        Cuenta.Text = "Entrada no válida";
                        return;
                    }

                    break;
                case "+/-":
                    if (!resultadoDecimal)
                    {
                        resultadoInt = -Int32.Parse(operador2);
                    }
                    else
                    {
                        resultadoDouble = -Double.Parse(operador2);
                    }

                    break;
            }

            if (!resultadoDecimal)
            {
                Cuenta.Text = resultadoInt.ToString();

            }
            else
            {
                Cuenta.Text = resultadoDouble.ToString();
            }
        }

        private void PulsarOperaciónDircta(object sender, RoutedEventArgs e)
        {
            OperacionDirecta(sender, e);
        }

        private void PulsarComa(object sender, RoutedEventArgs e)
        {
            AgregarComa();
        }

        private void PulsarIgual(object sender, RoutedEventArgs e)
        {
            RealizarCuenta();
            
        }

        private void PulsarCE(object sender, RoutedEventArgs e)
        {
            BorrarHistorialYCuenta();
        }

        private void PulsarDEL(object sender, RoutedEventArgs e)
        {
            BorrarUltimoDigito();
        }

        private void NumeroPulsado(object sender, RoutedEventArgs e)
        {
            AgregarNumero(((Button)sender).Content.ToString());
        }

        private void OperadorPulsado(object sender, RoutedEventArgs e)
        {
            AgregarOperador(sender, e);
        }


    }
}
