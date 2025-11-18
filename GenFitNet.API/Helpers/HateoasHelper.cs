using System.Text;

namespace GenFitNet.API.Helpers;

public static class HateoasHelper
{
    public static Dictionary<string, string> GenerateVagaLinks(int id, string baseUrl, string version)
    {
        return new Dictionary<string, string>
        {
            { "self", $"{baseUrl}/api/{version}/vagas/{id}" },
            { "update", $"{baseUrl}/api/{version}/vagas/{id}" },
            { "delete", $"{baseUrl}/api/{version}/vagas/{id}" },
            { "candidatos", $"{baseUrl}/api/{version}/candidatos?vagaId={id}" }
        };
    }

    public static Dictionary<string, string> GenerateCandidatoLinks(int id, string baseUrl, string version)
    {
        return new Dictionary<string, string>
        {
            { "self", $"{baseUrl}/api/{version}/candidatos/{id}" },
            { "update", $"{baseUrl}/api/{version}/candidatos/{id}" },
            { "delete", $"{baseUrl}/api/{version}/candidatos/{id}" }
        };
    }

    public static Dictionary<string, string> GeneratePagedLinks(
        int pageNumber, 
        int totalPages, 
        string baseUrl, 
        string version, 
        string resource)
    {
        var links = new Dictionary<string, string>
        {
            { "self", $"{baseUrl}/api/{version}/{resource}?pageNumber={pageNumber}" }
        };

        if (pageNumber > 1)
        {
            links.Add("first", $"{baseUrl}/api/{version}/{resource}?pageNumber=1");
            links.Add("prev", $"{baseUrl}/api/{version}/{resource}?pageNumber={pageNumber - 1}");
        }

        if (pageNumber < totalPages)
        {
            links.Add("next", $"{baseUrl}/api/{version}/{resource}?pageNumber={pageNumber + 1}");
            links.Add("last", $"{baseUrl}/api/{version}/{resource}?pageNumber={totalPages}");
        }

        return links;
    }
}

