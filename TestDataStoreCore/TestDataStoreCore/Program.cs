using System;
using System.Collections.Generic;
using Google.Cloud.Datastore.V1;

namespace TestDataStoreCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ajout de Personnes");
            CreatePersonnes();
            Console.WriteLine("Fini !!");
        }

        static void CreatePersonnes()
        {
            //renseignement de l'identifiant du projet sur Datastore
            string projectId = "testdatastore-297317";
            //on crée le genre Personne
            string kind = "Personne";
            //on crée la bd datastore
            var db = DatastoreDb.Create(projectId);


            List<Entity> personneEntities = new List<Entity>();

            personneEntities.Add(
                    new Entity
                    {
                        Key = db.CreateKeyFactory(kind).CreateKey($"key{1}"),
                        ["nom"] = $"Coudrec",
                        ["prenom"] = $"Clémentine",
                        ["age"] = 21
                    }
                );

            personneEntities.Add(
                    new Entity
                    {
                        Key = db.CreateKeyFactory(kind).CreateKey($"key{2}"),
                        ["nom"] = $"Balkany",
                        ["prenom"] = $"Patrick",
                        ["age"] = 65
                    }
                );

            //création de la transaction et envoie des personnes à datastore
            using (var transaction = db.BeginTransaction())
            {
                transaction.Upsert(personneEntities);
                transaction.Commit();
            }
        }
    }
}
