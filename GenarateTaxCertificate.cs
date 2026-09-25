using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GiftOfTheGivers.Functions;

public class TaxCertificateFunction
{
    private readonly ILogger<TaxCertificateFunction> _logger;

    public TaxCertificateFunction(ILogger<TaxCertificateFunction> logger)
    {
        _logger = logger;
    }

    [Function("GenerateTaxCertificate")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        DonationRequest? donation = null;
        try
        {
            donation = await req.ReadFromJsonAsync<DonationRequest>();
        }
        catch (JsonException)
        {
            // fall through to validation below
        }

        if (donation is null || string.IsNullOrWhiteSpace(donation.DonorName) || donation.Amount <= 0)
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("DonorName and an Amount greater than 0 are required.");
            return bad;
        }

        var certificate = new TaxCertificate
        {
            CertificateNumber = $"GOTG-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}",
            DonorName = donation.DonorName,
            DonorEmail = donation.DonorEmail,
            Amount = donation.Amount,
            Cause = donation.Cause ?? "General disaster relief",
            IssuedOn = DateTime.UtcNow,
            Issuer = "Gift of the Givers Foundation"
        };

        certificate.CertificateText =
            $"TAX CERTIFICATE\n" +
            $"Certificate No: {certificate.CertificateNumber}\n" +
            $"Issued by: {certificate.Issuer}\n" +
            $"This certifies that {certificate.DonorName} donated " +
            $"R{certificate.Amount:N2} towards {certificate.Cause} " +
            $"on {certificate.IssuedOn:dd MMMM yyyy}.\n" +
            $"(Dummy certificate for demonstration purposes.)";

        _logger.LogInformation("Generated certificate {Number} for {Donor}",
            certificate.CertificateNumber, certificate.DonorName);

        var ok = req.CreateResponse(HttpStatusCode.OK);
        await ok.WriteAsJsonAsync(certificate);
        return ok;
    }
}

public class DonationRequest
{
    public string DonorName { get; set; } = string.Empty;
    public string? DonorEmail { get; set; }
    public decimal Amount { get; set; }
    public string? Cause { get; set; }
}

public class TaxCertificate
{
    public string CertificateNumber { get; set; } = string.Empty;
    public string DonorName { get; set; } = string.Empty;
    public string? DonorEmail { get; set; }
    public decimal Amount { get; set; }
    public string Cause { get; set; } = string.Empty;
    public DateTime IssuedOn { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string CertificateText { get; set; } = string.Empty;
}
