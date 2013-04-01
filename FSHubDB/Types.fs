namespace FSHubDB

module Types =

    open System
    open MongoDB.Bson

        [<CLIMutableAttribute>]
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

        [<CLIMutableAttribute>]
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

