namespace FSHubDB

module Twitter =

    open System.IO
    open Twitterizer
    open Types
    open Utils
    open Mongo

    let blacklist =
        [|
            "Rishal Raja"
            "Bethany Gemma Hatton"
            "Joanne Duggan"
            "Nicola Bishop"
            "Phireside"
            "Jess Kenrick"
            "Jessica Forber"
            "mathew burrows"
            "USNewTrender"
            "Channing Tatum News"
            "ChicagoNewsNow"
            "not 'THE' les green"
            "Georgia Kelly"
            "Kristen Stewart"
            "Kieran Walsh"
            "Queerbo"
            "Mr. Lee Ti"
            "Cheralyn Roden"
            "Gerogie Charman"
            "Miles Folkes"
            "Gemma Cartwright"
            "Alan Bull"
            "Darryl Lawson"
            "Harriet Jackson"
            "Jordan"
            "X Factor 2012"
            "Martin McoY CottoN®"
            "Jamie McMurray"
            "Gaz_P"
            "Jake Sanders"
            "Ally Millar"
            "Laura Isabella"
            "Black_Bruce_Wayne"
            "Wesley C H Ma"
            "Randy Sterbentz"
            "Larissa de Oliveira"
            "Meagan Moore"
            "Brittany:*"
            "Rich Woodward"
            "aims."
            "Andrew Weikel"
            "ryan weikel"
            "shane moore"
            "Dave Coupland"
            "Phillip Mcgrouther"
            "bob marley"
            "Jordan Shelvey"
            "Ryan MacGillivray"
            "anna mae kamps"
            "Mae"
            "Sergio Bread&Water"
            "Lori Swankie"
            "Cris Myron Brenham"
        |]

    let notBlacklisted profile = Array.exists (fun x -> x = profile) blacklist = false
     
    let options = SearchOptions()
    options.NumberPerPage <- 50

    let latest() =
        try
            let search = TwitterSearch.Search("#fsharp", options)
            [for x in search.ResponseObject -> x]
            |> List.map (fun x ->
                let tweetId = x.Id.ToString()
                let userId = x.FromUserId.ToString()
                Tweet.Make tweetId userId x.ProfileImageLocation x.FromUserDisplayName x.FromUserScreenName x.CreatedDate x.Text)
            |> List.filter (fun x -> notBlacklisted x.DisplayName)
            |> Some
        with _ -> None

    let checkNewTweets datetime =
        let path = "Tweets.txt"
        let tweetsOption = latest()
        match tweetsOption with
            | None        -> ()
            | Some tweets ->
                let oldIds = File.ReadAllLines path
                let newTweets = tweets |> List.filter (fun x -> Array.exists (fun y -> y = x.TweetID) oldIds = false)
                let length = newTweets.Length
                match length with
                    | 0 -> ()
                    | _ ->
                        Collections.insertTweets newTweets
//                        newTweets |> List.iter (fun x -> printfn "%s" x.TweetID)
                        let ids = tweets |> List.map (fun x -> x.TweetID)
                        File.WriteAllLines(path, ids)
                        printfn "%s: %d new tweet(s)" datetime length