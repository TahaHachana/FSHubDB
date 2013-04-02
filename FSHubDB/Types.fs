namespace FSHubDB

module Types =

    open System
    open MongoDB.Bson

    [<CLIMutable>]
    type Question =
        {
            _id     : ObjectId
            Link    : string
            Title   : string
            Date    : DateTime
            Website : string
            Summary : string
        }

        static member Make link title date website summary =
            let summary' = match website with "FPish" -> "" | _ -> summary
            {
                _id     = ObjectId.GenerateNewId()
                Link    = link
                Title   = title
                Date    = date
                Website = website
                Summary = summary'
            }

    [<CLIMutable>]
    type Snippet =
        {
            _id         : ObjectId
            Link        : string
            Title       : string
            Description : string
            Date        : DateTime
        }

        static member Make link title description =
            {
                _id         = ObjectId.GenerateNewId()
                Link        = link
                Title       = title
                Description = description
                Date        = DateTime.Now
            }

    [<CLIMutable>]
    type Tweet =
        {
            _id           : ObjectId 
            TweetID       : string
            UserID        : string
            ProfileImage  : string
            DisplayName   : string
            ScreenName    : string
            CreationDate  : DateTime
            Text          : string
        }
    
        static member Make tweetId userId profileImage displayName screenName creationDate text =
            {
                _id           = ObjectId.GenerateNewId() 
                TweetID       = tweetId
                UserID        = userId
                ProfileImage  = profileImage
                DisplayName   = displayName
                ScreenName    = screenName
                CreationDate  = creationDate
                Text          = text
            }