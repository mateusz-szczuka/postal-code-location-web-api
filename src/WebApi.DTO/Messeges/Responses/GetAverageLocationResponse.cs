using WebApi.DTO.Messeges.Abstractions;

namespace WebApi.DTO.Messeges.Responses
{
	public sealed record GetAverageLocationResponse : ApiResponse
	{
        public double AverageLongitude { get; }

        public double AverageLatitude { get; }

        public int Count { get; }

        public GetAverageLocationResponse(double averageLongitude, double averageLatitude, int count)
        {
            AverageLatitude = averageLatitude;
            AverageLongitude = averageLongitude;
            Count = count;
        }

        public GetAverageLocationResponse(IFault fault) : base(fault)
        {
        }
    }
}