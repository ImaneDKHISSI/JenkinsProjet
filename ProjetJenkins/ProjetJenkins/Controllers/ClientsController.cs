using Microsoft.AspNetCore.Mvc;
using ProjetJenkins.Mapper;
using ProjetJenkins.Models;
using ProjetJenkins.ViewModels;

namespace ProjetJenkins.Controllers
{
    public class ClientsController : Controller
    {
        MyContext db;
        public ClientsController(MyContext db)
        {
            this.db = db;
        }
        public IActionResult AffichageClient()
        {
            List<Client> clients=db.Clients.ToList();
            return View(clients);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(ClientAddVM vm)
        {
            if(ModelState.IsValid)
            {
                Client client=ClientMapper.GetClientFromClientAddVM(vm);
               db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("AffichageClient");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            Client c=db.Clients.Where(c=>c.Id==id).FirstOrDefault();
            if(c!=null)
            {
                db.Clients.Remove(c);
                db.SaveChanges() ;
            }
            return RedirectToAction(nameof(AffichageClient));
        }
        public IActionResult Update(int id)
        {
            // Récupérer le client correspondant à l'ID
            Client client = db.Clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                // Rediriger si le client n'existe pas
                return RedirectToAction(nameof(AffichageClient));
            }

            // Mapper l'entité Client vers ClientUpdateVM pour passer au formulaire
            ClientUpdateVM viewModel = new ClientUpdateVM
            {
                Id = client.Id,
                Nom = client.Nom,
                Prenom = client.Prenom,
                Tel = client.Tel,
                CIN = client.CIN
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Update(ClientUpdateVM Cvm)
        {
            if (ModelState.IsValid)
            {
                // Récupérer le client existant dans la base de données
                Client client = db.Clients.FirstOrDefault(c => c.Id == Cvm.Id);
                if (client != null)
                {
                    // Mettre à jour directement les propriétés de l'objet existant
                    client.Nom = Cvm.Nom;
                    client.Prenom = Cvm.Prenom;
                    client.Tel = Cvm.Tel;
                    client.CIN = Cvm.CIN;

                    // Sauvegarder les modifications dans la base de données
                    db.SaveChanges();

                    // Rediriger vers la liste des clients après mise à jour
                    return RedirectToAction(nameof(AffichageClient));
                }

                // Si le client n'existe pas (cas rare), rediriger
                return RedirectToAction(nameof(AffichageClient));
            }

            // Si le modèle est invalide, renvoyer le ViewModel avec les erreurs de validation
            return View(Cvm);
        }



    }
}
