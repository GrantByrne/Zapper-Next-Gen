using FastEndpoints;
using FluentValidation;
using Zapper.Client.Devices;
using System.Net;
using System.Text.RegularExpressions;

namespace Zapper.API.Validators.Devices;

public class CreateDeviceRequestValidator : Validator<CreateDeviceRequest>
{
    public CreateDeviceRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Device name is required")
            .MaximumLength(100).WithMessage("Device name must not exceed 100 characters");

        RuleFor(x => x.Brand)
            .MaximumLength(50).WithMessage("Brand must not exceed 50 characters");

        RuleFor(x => x.Model)
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid device type");

        RuleFor(x => x.ConnectionType)
            .IsInEnum().WithMessage("Invalid connection type");

        RuleFor(x => x.IpAddress)
            .Must(BeAValidIpAddress)
            .When(x => !string.IsNullOrEmpty(x.IpAddress))
            .WithMessage("Invalid IP address format");

        RuleFor(x => x.MacAddress)
            .Must(BeAValidMacAddress)
            .When(x => !string.IsNullOrEmpty(x.MacAddress))
            .WithMessage("Invalid MAC address format");

        RuleFor(x => x.Port)
            .InclusiveBetween(1, 65535)
            .When(x => x.Port.HasValue)
            .WithMessage("Port must be between 1 and 65535");
    }

    private bool BeAValidIpAddress(string? ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
            return true;

        return IPAddress.TryParse(ipAddress, out _);
    }

    private bool BeAValidMacAddress(string? macAddress)
    {
        if (string.IsNullOrEmpty(macAddress))
            return true;

        var regex = new Regex(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
        return regex.IsMatch(macAddress);
    }
}