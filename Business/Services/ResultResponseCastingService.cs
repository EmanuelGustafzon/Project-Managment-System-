using Business.Interfaces;
using Business.Models;

namespace Business.Services;

public static class ResultResponseCastingService
{
    public static T? CastResultAndGetData<T>(IResponseResult result)
    {
        if (result is Result<T> successResult && successResult.Data is not null)
        {
            return successResult.Data;
        }
        return default;
    }
}
