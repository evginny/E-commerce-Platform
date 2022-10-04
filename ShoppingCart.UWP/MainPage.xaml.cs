using Library.ShoppingCart.Models;
using Library.ShoppingCart.Services;
using ShoppingCart.UWP.Dialogs;
using ShoppingCart.UWP.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ShoppingCart.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EmployeePage));
        }

        private void Customer_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CustomerPage));
        }
    }
}
