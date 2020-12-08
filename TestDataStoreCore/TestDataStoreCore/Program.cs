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
            GetPersonne();
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
                        Key = db.CreateKeyFactory(kind).CreateKey($"key{4}"),
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
        
        static void GetPersonne()
        {
            //Project Id from the Project at GCP 
            string projectId = "testdatastore-297317";
            //We are storing movies. So this is a Movie kind. 
            string kind = "Personne";
            //Create the datastore db
            var db = DatastoreDb.Create(projectId);
            Query query = new Query(kind);
            foreach (var personne in db.RunQueryLazily(query))
            {
                string line = $"La personne {personne["prenom"].StringValue} {personne["nom"].StringValue} est présente dans la base de donnée, elle a  {personne["age"].IntegerValue} ans.";

                Console.WriteLine(line);
            }
        }
    }
}
