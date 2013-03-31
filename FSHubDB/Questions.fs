namespace FSHubDB

module Questions =

    open Types

    module Stackoverflow =

        let uris =
            [|
                "http://stackoverflow.com/feeds/tag?tagnames=f%23&sort=newest"
                "http://stackoverflow.com/feeds/tag/f%23-interactive"
                "http://stackoverflow.com/feeds/tag/c%23-to-f%23"
                "http://stackoverflow.com/feeds/tag/f%23-3.0"
                "http://stackoverflow.com/feeds/tag/f%23-scripting"
                "http://stackoverflow.com/feeds/tag/f%23-powerpack"
            |]
    
        let collectQuestions uri = Rss.collectQuestions uri "entry" ["id"; "title"; "published"; "summary"] "StackOverflow"
    
        let questions () =
            uris
            |> Array.collect collectQuestions
            |> Seq.distinctBy (fun x -> x.Link)
            |> Seq.toArray

    module MSDN =

        let uri = "http://social.msdn.microsoft.com/Forums/en-US/fsharpgeneral/threads?outputAs=rss"
    
        let questions () = Rss.collectQuestions uri "item" ["guid"; "title"; "pubDate"; "description"] "MSDN"
  
    module FPish =

        let uri = "http://fpish.net/rss/topics/all"

        let questions () = Rss.collectQuestions uri "item" ["guid"; "title"; "pubDate"; "description"] "FPish"

    let collect() =
        let soQuestions = Stackoverflow.questions()
        let msdnQuestions = MSDN.questions()
        let fpishQuestions = FPish.questions()
        Array.concat [soQuestions; msdnQuestions; fpishQuestions]   
