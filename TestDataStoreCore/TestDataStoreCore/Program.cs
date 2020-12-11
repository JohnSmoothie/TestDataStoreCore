using System;
using System.Collections.Generic;
using Google.Cloud.Datastore.V1;

namespace TestDataStoreCore
{
    class Program
    {
            //renseignement de l'identifiant du projet sur Datastore
            private static string projectId = "testdatastore-297317";
            //on crée le genre Personne
            private static string kind = "Personne";

        static void Main(string[] args)
        {
            //Console.WriteLine("Ajout de Personnes");
            //CreatePersonnes();

            //Console.WriteLine("Recuperation de toutes les personnes");
            //GetAllPersonne();

            //Console.WriteLine("Recuperation d'une personne : ");
            //Get1Personne();

            //Console.WriteLine("Suppression d'une personne");
            //DeletePersonne();

            Console.WriteLine("Update des personnes");
            UpdatePersonne();
        }

        static void CreatePersonnes()
        {

            //on créé l'objet de service en mode Datastore autorisé
            var db = DatastoreDb.Create(projectId);

            List<Entity> personneEntities = new List<Entity>();

            personneEntities.Add(
                    new Entity
                    {
                        Key = db.CreateKeyFactory(kind).CreateKey($"key{1}"),
                        ["nom"] = $"Medori",
                        ["prenom"] = $"Loïc",
                        ["age"] = 24
                    }
                );

            personneEntities.Add(
                    new Entity
                    {
                        Key = db.CreateKeyFactory(kind).CreateKey($"key{2}"),
                        ["nom"] = $"Balkany",
                        ["prenom"] = $"Patrick",
                        ["age"] = 72
                    }
                );

            personneEntities.Add(
                    new Entity
                    {
                        Key = db.CreateKeyFactory(kind).CreateKey($"key{3}"),
                        ["nom"] = $"Bwa",
                        ["prenom"] = $"Viktor",
                        ["age"] = 21
                    }
                );

            //création de la transaction et envoie des personnes à datastore
            using (var transaction = db.BeginTransaction())
            {
                transaction.Upsert(personneEntities);
                transaction.Commit();
            }
        }
        
        static void GetAllPersonne()
        {
            //on créé l'objet de service en mode Datastore autorisé
            var db = DatastoreDb.Create(projectId);
            Query query = new Query(kind);
            foreach (var personne in db.RunQueryLazily(query))
            {
                string line = $"La personne {personne["prenom"].StringValue} {personne["nom"].StringValue} est présente dans la base de donnée, elle a  {personne["age"].IntegerValue} ans.";

                Console.WriteLine(line);
            }
        }
        static void Get1Personne()
        {
            //on créé l'objet de service en mode Datastore autorisé
            var db = DatastoreDb.Create(projectId);

            Query query = new Query("Personne")
            {
                Filter = Filter.Equal("nom", "Medori")
            };
            foreach (Entity entity in db.RunQueryLazily(query))
            {
                string prenom = (string)entity["prenom"];
                //string prenom = (string)entity["prenom"];
                Console.WriteLine($"{prenom}");
            }
        }
        static void DeletePersonne()
        {
            //on créé l'objet de service en mode Datastore autorisé
            var db = DatastoreDb.Create(projectId);

            Query query = new Query("Personne")
            {
                Filter = Filter.GreaterThan("age", 22),
            };
            foreach (Entity entity in db.RunQueryLazily(query))
            {
                string prenom = (string)entity["prenom"];
                if (entity != null)
                {
                    Console.WriteLine($"{prenom} a été supprimé");
                    db.Delete(entity);
                }
                
            }
        }
        static void UpdatePersonne()
        {
            //on créé l'objet de service en mode Datastore autorisé
            var db = DatastoreDb.Create(projectId);

            Query query = new Query("Personne")
            {
                Filter = Filter.Equal("nom", "Medori"),
            };
            foreach (Entity entity in db.RunQueryLazily(query))
            {
                string prenom = (string)entity["prenom"];
                if (entity != null)
                {
                    entity["age"] = 30;
                    Console.WriteLine($"{prenom} a été modifié.");
                    db.Update(entity);
                }

            }
        }
    }
}
