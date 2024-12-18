using Microsoft.AspNetCore.Mvc;

namespace Survey.Abstractions
{
    public static class ResultExtension
    {
        public static ObjectResult ToProblem(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Can not converted to problem");
            var problem = Results.Problem(statusCode : result.Error.StatueCode);
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
            problemDetails!.Extensions = new Dictionary<string, Object?>{
                {
                    "errors" , 
                    new [] {
                        result.Error.code,
                        result.Error.description
                    }
                }
            };
            return new ObjectResult(problemDetails);
        }
    }
}
