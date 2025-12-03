namespace ApiProject.Models
{
    public class ApiResponse<T>
    {
        public string status { get; set; }

        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }

        public bool displayMessage { get; set; }


        public static ApiResponse<T> SuccessResponse(T data, string? message = null)
        {
            return new ApiResponse<T>
            {
                status = "success",
                Success = true,
                Data = data,
                Message = message,
                displayMessage = false
            };
        }

        public static ApiResponse<T> ErrorResponse(string message
)
        {
            return new ApiResponse<T>
            {
                status = "failure",
                Success = false,
                Message = message,
                displayMessage = true


            };
        }
    }

    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalRecords { get; set; }
        public object Total { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

}
