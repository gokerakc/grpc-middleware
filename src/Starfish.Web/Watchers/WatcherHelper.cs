using System.Diagnostics;
using Microsoft.Extensions.Primitives;
using Starfish.Core.Resources;

namespace Starfish.Web.Watchers;

public static class WatcherHelper
{
    private static readonly Guid WatcherId = new("3E36307F-E703-420A-BB59-BEEE16717F26");
    public static void AddGuestListWatcher()
    {
        ChangeToken.OnChange(
            () => GuestListSource.Watch(WatcherId),
            () =>
            {
                Debug.WriteLine("Guest list has been changed");
                
                // Add handlers 
            });
    }
}