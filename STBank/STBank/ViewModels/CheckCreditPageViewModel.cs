using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using STBank.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace STBank.ViewModels
{
    public class CheckCreditPageViewModel : BindableBase,INavigatedAware
    {
        
        public string Dni { get; set; }
        public double Monto { get; set; }
        public string TipoCredito { get; set; }
        public List<string> TiposCredito { get; set; }
        public int NumeroCuotas { get; set; }
        public List<int> Cuotas { get; set; }
        public bool EstaAprobado { get; set; }
        public string Mensaje { get; set; }

        public ICommand EvaluarCommand { get; private set; }
        public ICommand RegistrarCommand { get; private set; }

        public CheckCreditPageViewModel(ICreditService creditService, INavigationService navigationService)
        {
            _creditService = creditService;
            _navigationService = navigationService;
            EvaluarCommand = new DelegateCommand(OnEvaludar);
            RegistrarCommand = new DelegateCommand(OnRegistrar, CanRegistrar).ObservesCanExecute(() => EstaAprobado);
        }

        private async void OnRegistrar()
        {
            var credito = new Credito()
            {
                Monto = Monto,
                Tipo = TipoCredito
            };

            if (await _creditService.RegistrarCreditoAsync(credito))
            {
                await _navigationService.NavigateAsync("/MainPage");
            }
        }

        private bool CanRegistrar()
        {
            return EstaAprobado;
        }

        private async void OnEvaludar()
        {
            var cliente = await _creditService.GetClienteByDNI(Dni);
            if (cliente == null)
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowError("La persona no es cliente.");
                return;
            }

            var credito = new Credito() 
            {
                Monto = Monto,
                Tipo = TipoCredito
            };
            
            EstaAprobado = await _creditService.EvaluarCreditoAsync(credito);
            Mensaje = EstaAprobado ? "Excelente, esta pre-aprobado":"Lo siento, no paso la evaluacion";
        }

        private readonly ICreditService _creditService;
        private readonly INavigationService _navigationService;

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            TiposCredito = (await _creditService.GetTipoCreditosAsync()).ToList();
        }
    }
}
