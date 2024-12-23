using ProjetJenkins.Models;
using ProjetJenkins.ViewModels;

namespace ProjetJenkins.Mapper
{
    public class ClientMapper
    {
        public static Client GetClientFromClientAddVM(ClientAddVM vm)
        {
            Client c=new Client();
            c.Nom=vm.Nom;
            c.Prenom=vm.Prenom;
            c.Tel=vm.Tel;
            c.CIN=vm.CIN;
            return c;
        }
        public static Client GetClientFromClientUpdateVM(ClientUpdateVM vm)
        {
            Client c = new Client();
            c.Id=vm.Id;
            c.Nom=vm.Nom;
            c.Prenom = vm.Prenom;
            c.Tel=vm.Tel;
            c.CIN=vm.CIN;
            return c;
        }
    }
}
