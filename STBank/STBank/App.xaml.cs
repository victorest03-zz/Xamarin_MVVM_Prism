﻿using Prism.Unity;
using STBank.Models;
using STBank.Views;
using Xamarin.Forms;
using Microsoft.Practices.Unity;
namespace STBank
{
    public partial class App : PrismApplication
    {        
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<CheckCreditPage>();
            Container.RegisterTypeForNavigation<CreditRegisteredPage>();

            Container.RegisterType<ICreditService, CreditService>();
        }
    }
}
