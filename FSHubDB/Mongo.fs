namespace FSHubDB

#if INTARCATIVE
#I """.\packages\mongocsharpdriver.1.8.0\lib\net35\"""
#r "MongoDB.Bson.dll"
#r "MongoDB.Driver.dll"
#endif

module Mongo =

    open System
    open System.Globalization
    open System.Threading
    open MongoDB.Bson
    open MongoDB.Driver
    open Types

    let culture = CultureInfo.CreateSpecificCulture("en-US")
    Thread.CurrentThread.CurrentCulture <- culture

    module Database =

        let connectionString = ""
        let client = MongoClient(connectionString)
        let server = client.GetServer()
        let database = server.GetDatabase "fsharpwebsite"
        let collectionByName<'T> (name : string) = database.GetCollection<'T>(name)

    module Collections =

        let questions = Database.collectionByName<Question> "questions"

        let insert (mongoCollection : MongoCollection) records = mongoCollection.InsertBatch records |> ignore

        let insertQuestions records = insert questions records