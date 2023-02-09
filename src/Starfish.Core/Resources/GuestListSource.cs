using Microsoft.Extensions.Primitives;

namespace Starfish.Core.Resources;

public static class GuestListSource
{
    private static readonly HashSet<string> Guests = new()
    {
        {"Jeramiah Novako"},
        {"Mina Ryan"},
        {"Serena Gillespie"},
        {"Marco Medina"}
    };
    
    public static IEnumerable<string> Get() => Guests;

    public static void Add(string fullName)
    {
        Guests.Add(fullName);
        Changed();
    }

    public static void Update(string oldFullname, string newFullname)
    {
        Remove(oldFullname);
        Add(newFullname);
        Changed();
    }

    public static void Remove(string fullName)
    {
        Guests.Remove(fullName);
        Changed();
    }

    public static void Clean()
    {
        Guests.Clear();
        Changed();
    }

    public static bool Exists(string fullName) => Guests.Contains(fullName);
    
    ///
    /// <summary>
    /// Rest of the class related to the tracking feature.
    /// If you need to track the guest list you can use the Watch() method.
    /// </summary>
    /// 
    private static CancellationTokenSource? _cancellationTokenSource;

    private static Guid? _watcherId;
    
    public static IChangeToken Watch(Guid watcherId)
    {
        if (_watcherId is not null && _watcherId != watcherId)
        {
            throw new ArgumentException($"There is already an active guest list watcher. watcherId: {_watcherId}");
        }

        _watcherId = watcherId;
        
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();

        var changeToken =  new CancellationChangeToken(_cancellationTokenSource.Token);

        return changeToken;
    }
    
    private static void Changed()
    {
        _cancellationTokenSource?.Cancel();
    }
}