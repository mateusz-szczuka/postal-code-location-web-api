using WebApi.Api.Extensions;
using WebApi.DTO.Messeges.Responses.Externals.Zippo;

namespace WebApi.Core.Extensions
{
	public static class LocationResponseExtensions
    {
        public static bool CheckIfLocationResultIsCorrect(this LocationResponse locationResponse)
            => locationResponse.IsNotFaulted() && locationResponse.PostalCode.IsNotNull() && locationResponse.PostalCode.Places.IsNotNullOrEmpty();
    }
}

