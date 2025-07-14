using System.Net.Http;
using System.Threading.Tasks;

namespace StudentManagement.Application.Helpers;

public class ExternalCpfValidator
{
    private readonly HttpClient _httpClient;

    public ExternalCpfValidator(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Valida o CPF usando a API pública do Speedio.
    /// Retorna true se o CPF existe na base externa.
    /// </summary>
    public async Task<bool> IsCpfValidAsync(string cpf)
    {
        var url = $"https://api-publica.speedio.com.br/buscarcpf?cpf={cpf}";
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return false;

        var content = await response.Content.ReadAsStringAsync();
        // A API retorna "error" se o CPF não existe
        return !content.Contains("\"error\"");
    }
} 