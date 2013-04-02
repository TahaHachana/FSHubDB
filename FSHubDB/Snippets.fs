namespace FSHubDB

module Snippets =

    open System.IO
    open FSharp.Data
    open FSharp.Net
    open Utils
    open Types
    open Mongo

    [<Literal>]
    let JsonSample =
        """[{ "author": "Kit Eason",
        "title": "Eurovision - Some(points)",
        "description": "The Eurovision final scoring system using records and some higher order functions. (...)",
        "likes": 1,
        "link": "http://fssnip.net/cg",
        "published": "5 months ago"},
        { "author": "Eirik Tsarpalis",
        "title": "Codomains through Reflection",
        "description": "Any type signature has the form of a curried chain T0 -> T1 -> .... -> Tn, where Tn is not a function type. (...)",
        "likes": 2,
        "link": "http://fssnip.net/cf",
        "published": "5 months ago" } ]"""

    type Fssnip = JsonProvider<JsonSample>

    let fetchJson() =
        Http.Request
            ( "http://api.fssnip.net/1/snippet", 
            headers=["content-type", "application/json"] )
    
//    let old 

    let checkNewSnippets datetime =
        let path = "Snippets.txt"
        let snippets =
            let json = fetchJson()
            Fssnip.Parse(json)

        let oldSnippets = File.ReadAllLines path    
        let newSnippets =
            snippets
            |> Array.filter (fun x -> Array.exists (fun y -> y = x.Link) oldSnippets = false)
        let count = newSnippets.Length
        match count with
            | 0 -> ()
            | _ ->
                let newSnippets' =
                    newSnippets
                    |> Array.map (fun x -> x.Link, x.Title, substring x.Description)
                    |> Array.map (fun (link, title, desc) -> Snippet.Make link title desc)
                    |> Collections.insertSnippets
//                    |> Array.iter (fun x -> printfn "%A" x)
                let links = snippets |> Array.map (fun x -> x.Link)
                File.WriteAllLines(path, links)
                printfn "%s: %d new snippet(s)" datetime count      
