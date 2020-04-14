using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MyViewModel __viewModel;


        public MainPage()
        {
            InitializeComponent();

            __viewModel = new MyViewModel();
            BindingContext = __viewModel;


            Button btn1 = this.FindByName<Button>("btn1");
            btn1.Command = __viewModel.MyCommand;

            Label lbl1 = this.FindByName<Label>("lbl1");
            lbl1.SetBinding(Label.TextProperty, new Binding() { Source = __viewModel, Path = "FactorialValue", Converter = new Utils.FactorialToStringConverter() });

            Entry entry1 = this.FindByName<Entry>("entry1");
            entry1.SetBinding(Entry.TextProperty, new Binding() { Source = __viewModel, Path = "Value", Converter = new Utils.StringToIntegerConverter() });
        }


        static async Task<int> Factorial(int val)
        {
            await Task.Delay(5000);

            int res = 1;
            for (int i = 2; i <= val; i++)
            {
                res *= i;
            }
            return res;
        }

        private class MyViewModel : Utils.ExtendedBindableObject
        {
            private bool _isCalculating = false;
            bool IsCalculating
            {
                get { return _isCalculating; }
                set
                {
                    if(_isCalculating != value)
                    {
                        _isCalculating = value;
                       // Console.WriteLine("--Called MyCommand.ChangeCanExecute()");
                    }
                }
            }
            public MyViewModel()
            {
                MyCommand = new Command(
                    execute: () => {

                        //Console.WriteLine("--Calculalting");
                        IsCalculating = true;
                        MyCommand.ChangeCanExecute();

                        Device.BeginInvokeOnMainThread(async () =>
                        {                            
                            try
                            {
                                FactorialValue = await Factorial(Value);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            finally
                            {
                                IsCalculating = false;
                                MyCommand.ChangeCanExecute();
                            }

                            //Console.WriteLine("--Finish Calculalting");
                            
                        });

                    },
                    canExecute: () => {
                        Console.WriteLine($"--canExecute {!IsCalculating}");
                        return !IsCalculating;
                    });
            }

            private int _factorialValue = 0;
            public int FactorialValue
            {
                get { return _factorialValue; }
                set
                {
                    if(_factorialValue != value)
                    {
                        _factorialValue = value;
                        this.RaisePropertyChanged<int>(() => FactorialValue);
                    }
                }
            }
            public int Value { get; set; }


            public Command MyCommand { get; set; }

        }
    }
}
