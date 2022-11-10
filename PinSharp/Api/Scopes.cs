using System;

namespace PinSharp.Api
{
    // TODO: Implement saving these with the access token and/or the PinSharpClient
    /// <summary>
    /// The different permission scopes you can request access when requesting an access token.
    /// </summary>
    public enum Scopes
    {
        // TODO: Implement this in PinSharpAuthClient
        None = 0,
        UserAccountsRead,
        AdsRead,
        BoardsRead,
        BoardsReadSecret,
        BoardsWrite,
        BoardsWriteSecret,
        PinsRead,
        PinsReadSecret,
        PinsWrite,
        PinsWriteSecret
    }
}
