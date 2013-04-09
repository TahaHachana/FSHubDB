namespace FSHubDB

module Utils =

    open System.Text.RegularExpressions

    let charRegex = Regex("\p{P}? .{1}", RegexOptions.Compiled ||| RegexOptions.RightToLeft)

    let stripTrailingChars str =
        charRegex.Replace(str, "",1)
        |> fun x -> x + " ..."

    let substring (str : string) =
        let length = str.Length
        match length > 150 with
            | false -> str
            | true  -> str.Substring(0, 150) |> stripTrailingChars
