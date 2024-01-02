using WebApi.DTO.Messeges.Abstractions;
using WebApi.DTO.Models.Externals.Zippo;

namespace WebApi.DTO.Messeges.Responses.Externals.Zippo;

public sealed record LocationResponse : ApiResponse
{
    public PostalCode PostalCode { get; }

    public LocationResponse(PostalCode postalCode)
    {
        PostalCode = postalCode;
    }

    public LocationResponse(IFault fault) : base(fault)
    {
    }
}