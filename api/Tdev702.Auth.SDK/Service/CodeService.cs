using System.Collections.Concurrent;

namespace Tdev702.Auth.SDK.Service;

public interface ICodeStateService
{
    Task<string> GetLastGeneratedCode(string email);
    void SetGeneratedCode(string email, string code);
}

public class CodeStateService : ICodeStateService
{
    private readonly ConcurrentDictionary<string, string> _codes = new();
    
    public async Task<string> GetLastGeneratedCode(string email)
    {
        await Task.Delay(2000);
        return _codes.TryGetValue(email, out var code) ? code : null;
    }
    
    public void SetGeneratedCode(string email, string code)
    {
        _codes[email] = code;
    }
}