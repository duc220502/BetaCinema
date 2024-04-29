using Azure;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface ICinemaService
    {
        ResponseObject<DataResponseCinema> CreatCinema(Request_AddCinema rq);
        ResponseObject<DataResponseCinema> UpdateCinema(int id,Request_UpdateCinema rq);

        ResponseObject<DataResponseCinema> DeleteCinema(int id);

        ResponseObject<double> Get_Revenue(int CinemaId,DateTime d1,DateTime d2);
    }
}
