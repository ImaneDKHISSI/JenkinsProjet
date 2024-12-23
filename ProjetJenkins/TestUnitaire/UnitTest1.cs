using Microsoft.EntityFrameworkCore;
using ProjetJenkins.Controllers;
using ProjetJenkins.Models;
using ProjetJenkins.ViewModels;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TestUnitaire
{
    public class UnitTest1
    {
        private readonly ClientsController _controller;
        private readonly MyContext _context;

        public UnitTest1()
        {
            // Configurer la base de données en mémoire
            var options = new DbContextOptionsBuilder<MyContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new MyContext(options);

            // Ajouter des données fictives pour les tests
            _context.Clients.AddRange(
                 new Client { Nom = "Doe", Prenom = "John", Tel = "123456789", CIN = "CIN123" },
                 new Client { Nom = "Smith", Prenom = "Jane", Tel = "987654321", CIN = "CIN456" }
            );
            _context.SaveChanges();

            // Initialiser le contrôleur
            _controller = new ClientsController(_context);
        }

        private void CleanDatabase()
        {
            _context.Clients.RemoveRange(_context.Clients);
            _context.SaveChanges();
        }

        private void PrintClients(string message)
        {
            var clients = _context.Clients.ToList();
            Debug.WriteLine(message);
            foreach (var client in clients)
            {
                Debug.WriteLine($"Client Id: {client.Id}, Nom: {client.Nom}");
            }
        }
        [Fact]
        public void AffichageClient_ReturnsViewWithClients()
        {
            // Nettoyer la base de données
            CleanDatabase();

            // Arrange
            _context.Clients.AddRange(
                new Client { Nom = "Doe", Prenom = "John", Tel = "123456789", CIN = "CIN123" },
                new Client { Nom = "Smith", Prenom = "Jane", Tel = "987654321", CIN = "CIN456" }
            );
            _context.SaveChanges();

            // Act
            var result = _controller.AffichageClient();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Client>>(viewResult.Model);
            Assert.Equal(2, model.Count); // Vérifie qu'il y a deux clients
        }


        [Fact]
        public void Add_ValidClient_RedirectsToAffichageClient()
        {
            // Nettoyer la base de données
            CleanDatabase();

            // Déboguer avant
            PrintClients("Avant l'ajout");

            // Arrange
            var vm = new ClientAddVM
            {
                Nom = "Brown",
                Prenom = "Chris",
                Tel = "555555555",
                CIN = "CIN789"
            };

            // Act
            var result = _controller.Add(vm);

            // Déboguer après
            PrintClients("Après l'ajout");

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result); // Vérifie que la redirection fonctionne
            Assert.Equal("AffichageClient", redirectResult.ActionName); // Vérifie la bonne redirection

            var addedClient = _context.Clients.FirstOrDefault(c => c.CIN == "CIN789");
            Assert.NotNull(addedClient); // Vérifie que le client existe
            Assert.Equal("Brown", addedClient.Nom); // Vérifie le nom du client
            Assert.Equal(1, _context.Clients.Count()); // Vérifie qu'il y a bien 1 client après l'ajout
        }

        [Fact]
        public void Add_InvalidModel_ReturnsView()
        {
            // Nettoyer la base de données
            CleanDatabase();

            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid Model"); // Simule un modèle invalide

            var vm = new ClientAddVM
            {
                Nom = "", // Nom vide pour invalider le modèle
                Prenom = "Chris",
                Tel = "555555555",
                CIN = "CIN789"
            };

            // Act
            var result = _controller.Add(vm);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); // Vérifie que la vue est retournée
            Assert.Null(_context.Clients.FirstOrDefault(c => c.CIN == "CIN789")); // Vérifie qu'aucun client n'a été ajouté
            Assert.Equal(0, _context.Clients.Count()); // Vérifie que la base est toujours vide
        }



        [Fact]
        public void Delete_ExistingClient_RemovesClientAndRedirects()
        {
            // Nettoyer la base de données
            CleanDatabase();

            // Arrange - Ajouter des données initiales
            var client1 = new Client { Nom = "Doe", Prenom = "John", Tel = "123456789", CIN = "CIN123" };
            var client2 = new Client { Nom = "Smith", Prenom = "Jane", Tel = "987654321", CIN = "CIN456" };

            _context.Clients.AddRange(client1, client2);
            _context.SaveChanges();

            // Déboguer avant suppression
            Debug.WriteLine("Avant suppression :");
            foreach (var client in _context.Clients.ToList())
            {
                Debug.WriteLine($"Client Id: {client.Id}, Nom: {client.Nom}");
            }

            // Act - Supprimer le premier client
            var result = _controller.Delete(client1.Id);

            // Déboguer après suppression
            Debug.WriteLine("Après suppression :");
            foreach (var client in _context.Clients.ToList())
            {
                Debug.WriteLine($"Client Id: {client.Id}, Nom: {client.Nom}");
            }

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AffichageClient", redirectResult.ActionName);

            // Vérifier qu'il ne reste qu'un seul client
            Assert.Single(_context.Clients);
            Assert.DoesNotContain(_context.Clients, c => c.Id == client1.Id); // Vérifie que le client supprimé n'existe plus
        }



        [Fact]
        public void Update_ValidClient_UpdatesAndRedirects()
        {
            // Arrange
            var client = new Client { Nom = "Doe", Prenom = "John", Tel = "123456789", CIN = "CIN123" };

            _context.Clients.Add(client);
            _context.SaveChanges(); // Ajouter un client initial

            var vm = new ClientUpdateVM
            {
                Id = client.Id, // Utiliser l'ID du client existant
                Nom = "Updated",
                Prenom = "Client",
                Tel = "987654321",
                CIN = "CIN999"
            };

            // Act
            var result = _controller.Update(vm);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result); // Vérifie que la redirection fonctionne
            Assert.Equal("AffichageClient", redirectResult.ActionName); // Vérifie la bonne redirection

            var updatedClient = _context.Clients.FirstOrDefault(c => c.Id == vm.Id);
            Assert.NotNull(updatedClient); // Vérifie que le client mis à jour existe
            Assert.Equal("Updated", updatedClient.Nom); // Vérifie que le nom a été mis à jour
            Assert.Equal("987654321", updatedClient.Tel); // Vérifie que le numéro de téléphone a été mis à jour
            Assert.Equal("CIN999", updatedClient.CIN); // Vérifie que le CIN a été mis à jour
        }


        [Fact]
        public void Update_InvalidModel_ReturnsView()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid Model");

            var vm = new ClientUpdateVM
            {
                Id = 1,
                Nom = "",
                Prenom = "John",
                Tel = "123123123",
                CIN = "CIN123"
            };

            // Act
            var result = _controller.Update(vm);

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
