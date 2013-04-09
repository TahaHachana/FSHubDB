namespace FSHubDB

module Rss =

    open System
    open System.Net
    open System.Text.RegularExpressions
    open System.Xml.Linq
    open Utils
    open Types

    let load (uri : string) = try XDocument.Load uri |> Some with _ -> None

    let elementsByName (xdocument : XDocument) localName =
        query {
            for x in xdocument.Descendants() do
                where (x.Name.LocalName = localName)
                select x
        } |> Seq.toArray

    let elementsValues (element : XElement) localNames =
        let descendants = element.Descendants()
        localNames
        |> List.map (fun localName ->
            descendants |> Seq.find (fun x -> x.Name.LocalName = localName)
            |> fun x -> x.Value)

    let tagRegex = Regex("<[^>]+>", RegexOptions.Compiled)
    let stripTags str = tagRegex.Replace(str, "").Trim()

    let charRegex = Regex("\p{P}? .{1}", RegexOptions.Compiled ||| RegexOptions.RightToLeft)
    let removeTrailingChars str =
        charRegex.Replace(str, "",1)
        |> fun x -> x + " ..."

    let substring (str : string) =
        let length = str.Length
        match length > 150 with
            | false -> str
            | true  -> str.Substring(0, 150) |> removeTrailingChars

    let processRssItem website (lst : string list) =
//        let title' = Utilities.substring lst.[1]
        let date' = DateTime.Parse lst.[2]
        let summary' =
            WebUtility.HtmlDecode lst.[3]
            |> stripTags
            |> substring
        lst.[0], lst.[1], date', website, summary'

    let processRss (rssOption : XDocument option) elem localNames website =
        match rssOption with
            | None -> [||]
            | Some rss ->    
                elementsByName rss elem
                |> Array.map (fun x -> elementsValues x localNames)
                |> Array.map (processRssItem website)
                |> Array.map (fun (link, title, date, website, summary) -> Question.Make link title date website summary)

    let collectQuestions uri elem localNames website =
        let rssOption = load uri
        processRss rssOption elem localNames website


